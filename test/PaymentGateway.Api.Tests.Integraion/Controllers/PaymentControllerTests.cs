using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using PaymentGateway.Models.Contracts;
using Xunit;

namespace PaymentGateway.Api.Tests.Integration.Controllers
{
    public class PaymentControllerTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly CustomWebAppFactory _webApplicationFactory;

        public PaymentControllerTests(CustomWebAppFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }


        [Theory]
        [InlineData("k")]
        [InlineData("null")]
        [InlineData("1231a")]
        [InlineData("!!#~")]
        public async Task GetTransaction_returns_bad_request(string id)
        {
            var client = _webApplicationFactory.CreateClient();

            var response = await client.GetAsync($"api/payment/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await ReadAsAsync<ValidationProblemDetails>(response.Content);

            body.Should().BeOfType<ValidationProblemDetails>();
        }

        [Theory]
        [MemberData(nameof(CreatePaymentTestData))]
        public async Task Create_payment_validates_request_body(
            (CreatePaymentRequest request, ValidationProblemDetails error) testData)
        {
            var client = _webApplicationFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
            var response = await client.PostAsync("api/payment",
                new StringContent(JsonSerializer.Serialize(testData.request), Encoding.Default,
                    MediaTypeNames.Application.Json));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await ReadAsAsync<ValidationProblemDetails>(response.Content);
            body.Should().BeOfType<ValidationProblemDetails>();
        }

        [Theory]
        [MemberData(nameof(ValidatationAttributeTestData))]
        public static void Create_payment_attributes_validate_request(
            (CreatePaymentRequest request, string error) testData)
        {
            DateTimeProvider.SetDateTime(new DateTime(2020, 10, 01));
            var request = testData.request;

            var validationResults = request.Validate(new ValidationContext(request)).ToList();
            validationResults.FirstOrDefault(x => x.ErrorMessage == testData.error).Should().NotBeNull();
        }

        public static TheoryData<(CreatePaymentRequest request, string error)> ValidatationAttributeTestData =
            new TheoryData<(CreatePaymentRequest request, string error)>
            {
                (new CreatePaymentRequest
                {
                    ExpiryYear = "kk",
                    ExpiryMonth = "01",
                    Cvv = "1234",
                    Amount = "10.10",
                    CardNumber = "1234567891234567"
                }, "The computed card expiry date is invalid"),
                (new CreatePaymentRequest
                {
                    ExpiryYear = "19",
                    Cvv = "1234",
                    Amount = "10.10",
                    CardNumber = "1234567891234567"
                }, "The expiry year is in the past"),
                (new CreatePaymentRequest
                {
                    ExpiryYear = "20", ExpiryMonth = "09",
                    Cvv = "1234",
                    Amount = "10.10",
                    CardNumber = "1234567891234567"
                }, "The computed expiry date is in the past"),
                (new CreatePaymentRequest
                    {
                        ExpiryYear = "21", ExpiryMonth = "01", Amount = "-1.00",
                        Cvv = "1234",

                        CardNumber = "1234567891234567"
                    },
                    "The field must be greater than 00.00")
            };

        public static TheoryData<(CreatePaymentRequest, ValidationProblemDetails)> CreatePaymentTestData =
            new TheoryData<(CreatePaymentRequest, ValidationProblemDetails)>
            {
                (new CreatePaymentRequest
                {
                    Amount = "10.10",
                    CardNumber = "1234567891123456",
                    Cvv = "1234",
                    ExpiryYear = "02",
                    ExpiryMonth = "20",
                    Name = "tom b"
                }, new ValidationProblemDetails())
            };


        [Fact]
        public async Task Create_payment_returns_ok()
        {
            var nextYear = DateTime.Now.AddYears(1);

            var request = new CreatePaymentRequest
            {
                Amount = "20.68",
                Cvv = "2345",
                Name = "Tom Biddulph",
                CardNumber = "123456789012345",
                ExpiryMonth = nextYear.ToString("MM"),
                ExpiryYear = nextYear.ToString("yy")
            };

            var client = _webApplicationFactory.CreateDefaultClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
            var response = await client.PostAsync("api/payment",
                new StringContent(JsonSerializer.Serialize(request), Encoding.Default,
                    MediaTypeNames.Application.Json));

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Should().Contain(x => x.Key == "Location");

            var locationHeader = response.Headers.GetValues("Location").First();

            using var jdoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

            var idProp = jdoc.RootElement.GetProperty("id");
            idProp.GetGuid().Should().NotBeEmpty("Each payment has a unique id");
            jdoc.RootElement.GetProperty("status").GetString().Should().Be("Success");
            locationHeader.Should().EndWith(idProp.GetString());
        }

        [Fact]
        public async Task Get_transaction_returns_transaction()
        {
            var id = Guid.Parse("59ead15a-f908-4a66-9be3-4967520c621e");
            var client = _webApplicationFactory.CreateDefaultClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            var response = await client.GetAsync($"api/payment/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var jdoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

            jdoc.RootElement.GetProperty("id").GetGuid().Should().Be(id);
            jdoc.RootElement.GetProperty("status").GetString().Should().Be("Success");
            jdoc.RootElement.GetProperty("amount").GetDecimal().Should().Be(1234.93m);
            jdoc.RootElement.GetProperty("cardNumber").GetString().Should().Be("************1234");
        }

        private static async Task<T> ReadAsAsync<T>(HttpContent content)
        {
            var str = await content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<T>(str);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
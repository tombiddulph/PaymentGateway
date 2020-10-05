using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

namespace PaymentGateway.Api.IntegrationTests.Controllers
{
    public class PaymentControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public PaymentControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory]
        [InlineData("k")]
        [InlineData("null")]
        [InlineData("1231a")]
        [InlineData("!!#~")]
        [InlineData(null)]
        public async Task GetTransaction_returns_bad_request(string id)
        {
            var client = _webApplicationFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/payment/transaction?Id={id}");
            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await ReadAsAsync<ValidationProblemDetails>(response.Content);

            body.Should().BeOfType<ValidationProblemDetails>();
        }



        private static async Task<T>ReadAsAsync<T>(HttpContent content)
        {
            var str = await content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<T>(str);
            }
            catch (Exception e)
            {
                return default;
            }
        }

    }
}
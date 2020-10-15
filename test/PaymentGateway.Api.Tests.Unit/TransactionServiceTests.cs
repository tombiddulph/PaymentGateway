using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using PaymentGateway.Application.Infrastructure;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Services;
using PaymentGateway.Models.Contracts;
using Xunit;
using PaymentStatus = PaymentGateway.Models.Enums.PaymentStatus;

namespace PaymentGateway.Api.Tests.Unit
{
    public class TransactionServiceTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        [Fact]
        public void Constructor_guards_parameter()
        {
            new GuardClauseAssertion(_fixture).Verify(typeof(TransactionService).GetConstructors());
        }

        [Theory]
        [InlineData(nameof(TransactionService.CreateTransactionAsync))]
        [InlineData(nameof(TransactionService.GetById))]
        public void Methods_guard_parameters(string methodName)
        {
            var assertion = new GuardClauseAssertion(_fixture);
            var method = typeof(TransactionService).GetMethod(methodName);
            assertion.Verify(method);
        }

        [Fact]
        public async Task CreateTransaction_returns_transaction()
        {
            var repo = Mock.Of<IRepository<Transaction>>();
            var paymentId = Guid.NewGuid();
            Mock.Get(repo).Setup(x => x.AddAsync(It.IsAny<Transaction>())).ReturnsAsync(paymentId);

            var sut = new TransactionService(repo);

            var request = _fixture.Create<CreatePaymentRequest>();

            var result = await sut.CreateTransactionAsync(request);

            result.Id.Should().Be(paymentId);
            result.Status.Should().Be(PaymentStatus.Success);
            result.Amount.Should().BeNull();
            result.CardNumber.Should().BeNull();
        }

        [Fact]
        public async Task CreateTransaction_bubbles_up_exception()
        {
            var repo = Mock.Of<IRepository<Transaction>>();
            Mock.Get(repo).Setup(x => x.AddAsync(It.IsAny<Transaction>())).ThrowsAsync(new InvalidOperationException());


            var sut = new TransactionService(repo);

            Func<Task> act = async () => await sut.CreateTransactionAsync(_fixture.Create<CreatePaymentRequest>());

            await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        }


        [Fact]
        public async Task Get_returns_not_found()
        {
            var repo = Mock.Of<IRepository<Transaction>>();
            var paymentId = Guid.NewGuid();
            Mock.Get(repo).Setup(x => x.GetByIdAsync(paymentId)).ReturnsAsync((Transaction) null);

            var sut = new TransactionService(repo);

            var result = await sut.GetById(paymentId);


            var expectedTransaction = new Models.Domain.Transaction
            {
                Id = null,
                Status = PaymentStatus.NotFound,
                Amount = null,
                CardNumber = null
            };


            result.Should().BeEquivalentTo(expectedTransaction);
        }
    }
}
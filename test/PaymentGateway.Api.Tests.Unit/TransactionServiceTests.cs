using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using PaymentGateway.Application.Services;
using Xunit;

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
    }
}
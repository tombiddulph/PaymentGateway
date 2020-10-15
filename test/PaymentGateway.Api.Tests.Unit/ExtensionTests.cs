using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using PaymentGateway.Application;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Services;
using Xunit;

namespace PaymentGateway.Api.Tests.Unit
{
    public class ExtensionTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Fact]
        public void Mask_card_number_guards_parameters()
        {
            var assertion = new GuardClauseAssertion(_fixture);
            var method = typeof(Extensions).GetMethod(nameof(Extensions.MaskCardNumber));
            assertion.Verify(method);
        }
        [Fact]
        public void Mask_card_number_returns_masked_number()
        {
            var card = new Card
            {
                Number = "4004400440044004"
            };

            var masked = card.MaskCardNumber();

            masked.Should().Be("************4004");
        }


        [Fact]
        public void InvokeSave_guards_parameters()
        {
            var assertion = new GuardClauseAssertion(_fixture);
            var method = typeof(Extensions).GetMethod(nameof(Extensions.SafeInvoke));
            assertion.Verify(method);
        }

        [Fact]
        public void Invoke_safe_calls_error_handler()
        {
            Func<string, string> sut = s => throw new InvalidOperationException("Invalid operation");

            Action<Exception> handler = Mock.Of<Action<Exception>>();


            Mock.Get(handler).Setup(x => x(It.IsAny<Exception>()));

            Action act = () => sut.SafeInvoke(handler, string.Empty);

            act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Invalid operation");
            
            Mock.Get(handler)
                .Verify(x => x(It.Is<InvalidOperationException>(y => y.Message == "Invalid operation")),
                    Times.Once);
        }

        [Fact]
        public void Invoke_safe_calls_func()
        {
            Func<int, int> sut = i => i * i;
            Action<Exception> handler = Mock.Of<Action<Exception>>();

            var result = sut.SafeInvoke(handler, 10);

            result.Should().Be(100);
            Mock.Get(handler)
                .Verify(x => x(It.IsAny<Exception>()),
                    Times.Never);
            
        }
    }
}
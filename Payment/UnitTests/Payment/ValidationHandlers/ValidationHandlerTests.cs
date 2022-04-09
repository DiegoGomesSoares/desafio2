using AutoFixture.Idioms;
using Domain.Entities;
using Domain.Models;
using Domain.Validators;
using FluentAssertions;
using NSubstitute;
using Payment.ValidationHandlers;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.ValidationHandlers
{
    public class ValidationHandlerTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ValidationHandler).GetConstructors());
        }

        [Fact]
        public void Sut_ShouldImplement_IValidationHandler()
        {
            typeof(ValidationHandler).Should().BeAssignableTo<IValidationHandler>();
        }

        [Theory, AutoNSubstituteData]
        public void Handle_WhenIsCashinOperation_ShouldCallCashinValidator(
            Conta account,
            ValidateResultModel validationResult,
            ValidationHandler sut)
        {
            var amount = 2;
            sut.AccountTransactionValidator
                .ValidateCashinOperation(Arg.Any<Conta>(), Arg.Any<decimal>())
                .Returns(validationResult);

            var actual = sut.Handle(account, amount);

            actual.Should().Be(validationResult);

            sut.AccountTransactionValidator.Received().ValidateCashinOperation(account, amount);
            sut.AccountTransactionValidator.DidNotReceive().ValidateCashOutOperation(Arg.Any<Conta>(), Arg.Any<decimal>());
        }

        [Theory, AutoNSubstituteData]
        public void Handle_WhenIsCasOutOperation_ShouldCallCahoutValidator(
            Conta account,
            ValidateResultModel validationResult,
            ValidationHandler sut)
        {
            var amount = -2;
            sut.AccountTransactionValidator
                .ValidateCashOutOperation(Arg.Any<Conta>(), Arg.Any<decimal>())
                .Returns(validationResult);

            var actual = sut.Handle(account, amount);

            actual.Should().Be(validationResult);

            sut.AccountTransactionValidator.Received().ValidateCashOutOperation(account, amount);
            sut.AccountTransactionValidator.DidNotReceive().ValidateCashinOperation(Arg.Any<Conta>(), Arg.Any<decimal>());
        }
    }
}

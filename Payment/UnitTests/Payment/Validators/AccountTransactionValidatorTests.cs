using AutoFixture.Idioms;
using Domain.Entities;
using Domain.Validators;
using FluentAssertions;
using Payment.Validators;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Validators
{
    public class AccountTransactionValidatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AccountTransactionValidator).GetConstructors());
        }

        [Fact]
        public void Sut_ShouldImplement_IAccountTransactionValidator()
        {
            typeof(AccountTransactionValidator).Should().BeAssignableTo<IAccountTransactionValidator>();
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashinOperation_WhenAccountIsNull_ShouldReturnAccountNotFoundErrorMessage(
            decimal amount,
            AccountTransactionValidator sut)
        {
            var actual = sut.ValidateCashinOperation(null, amount);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Account Not Found";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashinOperation_WhenAccountIsNotEnabled_ShouldReturnAccountIsBlockedErrorMessage(
            Conta account,
            decimal amount,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = false;
            var actual = sut.ValidateCashinOperation(account, amount);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Account Is Blocked";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashinOperation_WhenIsAnInvalidCashinAmount_ShouldReturnInvalidAmountCashinErrorMessage(
            Conta account,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = true;
            var amount = -1;
            var actual = sut.ValidateCashinOperation(account, amount);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Amount Must be greater then 0";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashinOperation_IsValid_ShouldReturnValid(
            Conta account,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = true;
            var amount = 1;
            var actual = sut.ValidateCashinOperation(account, amount);

            actual.IsValid.Should().BeTrue();
            actual.ErrorMessage.Should().BeNull();
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashOutOperation_WhenAccountIsNull_ShouldReturnAccountNotFoundErrorMessage(
            decimal amount,
            AccountTransactionValidator sut)
        {
            var actual = sut.ValidateCashOutOperation(null, amount);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Account Not Found";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashOutOperation_WhenAccountIsNotEnabled_ShouldReturnAccountIsBlockedErrorMessage(
            Conta account,
            decimal amount,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = false;
            var actual = sut.ValidateCashOutOperation(account, amount);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Account Is Blocked";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashOutOperation_WhenIsAnInvalidCashoutAmount_ShouldReturnInvalidAmountCashoutErrorMessage(
            Conta account,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = true;
            var amount = 1;
            var actual = sut.ValidateCashOutOperation(account, amount);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Amount Must be less than 0";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashOutOperation_WhenCashoutAmountIsGreaterThenlimit_ShouldReturnInvalidAmountCashOutDalyLimit(
            Conta account,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = true;
            account.LimiteSaqueDiario = 500;
            var amount = -501;
            var actual = sut.ValidateCashOutOperation(account, amount);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Amount Must Not to be greater then CashOutDailyLimit";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoNSubstituteData]
        public void ValidateCashOutOperation_WhenAccountDoesNotHaveCashOutLimit_ShouldReturnValid(
            Conta account,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = true;
            account.LimiteSaqueDiario = null;
            var amount = -500;
            var actual = sut.ValidateCashOutOperation(account, amount);

            actual.IsValid.Should().BeTrue();
            actual.ErrorMessage.Should().BeNull();
        }

        [Theory]
        [AutoInlineData(500)]
        [AutoInlineData(501)]
        public void ValidateCashOutOperation_WhenCashoutAmountIsLessThenlimit_ShouldReturnValid(
            decimal cashOutDayliLimit,
            Conta account,
            AccountTransactionValidator sut)
        {
            account.FlagAtivo = true;
            account.LimiteSaqueDiario = cashOutDayliLimit;
            var amount = -500;
            var actual = sut.ValidateCashOutOperation(account, amount);

            actual.IsValid.Should().BeTrue();
            actual.ErrorMessage.Should().BeNull();
        }
    }
}

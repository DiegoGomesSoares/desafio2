using AutoFixture.Idioms;
using Domain.Validators;
using FluentAssertions;
using Payment.Validators;
using UnitTests.AutoData;
using Xunit;


namespace UnitTests.Payment.Validators
{
    public class AccountIdValidatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AccountIdValidator).GetConstructors());
        }

        [Fact]
        public void Sut_ShouldImplement_AccountIdValidator()
        {
            typeof(AccountIdValidator).Should().BeAssignableTo<IAccountIdValidator>();
        }

        [Theory]
        [AutoInlineData(0)]
        [AutoInlineData(-2)]
        public void Validate_WhenAccountIsInvalid_ShouldReturnInvalidAccountErrorMessage(
           int accountId,
           AccountIdValidator sut)
        {
            var actual = sut.Validate(accountId);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Invalid AccountId";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoInlineData]
        public void Validate_WhenAccountIsValid_ShouldReturnValid(
           AccountIdValidator sut)
        {
            var actual = sut.Validate(2);

            actual.IsValid.Should().BeTrue();

            actual.ErrorMessage.Should().BeNull();
        }
    }
}

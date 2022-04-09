using AutoFixture.Idioms;
using Domain.Entities;
using Domain.Enums;
using Domain.Validators;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Payment.Validators;
using System.Threading.Tasks;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Validators
{
    public class AccountCreateValidatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AccountCreateValidator).GetConstructors());
        }

        [Fact]
        public void Sut_ShouldImplement_IAccountCreateValidator()
        {
            typeof(AccountCreateValidator).Should().BeAssignableTo<IAccountCreateValidator>();
        }

        [Theory, AutoNSubstituteData]
        public async Task ValidateAsync_WhenPersonIsInvalid_ShouldReturnInvalidPersonErrorMessage(
            int personId,
            AccountTypeEnum accountType,
            AccountCreateValidator sut)
        {
            sut.PersonReader.GetPersonByIdAsync(Arg.Any<int>()).ReturnsNull();

            var actual = await sut.ValidateAsync(personId, accountType);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Invalid Person";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);

            await sut.PersonReader.Received().GetPersonByIdAsync(personId);
            await sut.AccountReader.DidNotReceive()
                    .GetAccountByPersonIdAndAccountTypeAsync(Arg.Any<int>(), Arg.Any<AccountTypeEnum>());
        }

        [Theory, AutoNSubstituteData]
        public async Task ValidateAsync_WhenPersonAlreadyHasAnAccountWithSameType_ShouldReturnInvalidAccountTypeErrorMessage(
            int personId,
            AccountTypeEnum accountType,
            Pessoa pessoa,
            Conta conta,
            AccountCreateValidator sut)
        {
            sut.PersonReader.GetPersonByIdAsync(Arg.Any<int>()).Returns(pessoa);
            sut.AccountReader
                .GetAccountByPersonIdAndAccountTypeAsync(
                    Arg.Any<int>(), Arg.Any<AccountTypeEnum>()).Returns(conta);

            var actual = await sut.ValidateAsync(personId, accountType);

            actual.IsValid.Should().BeFalse();

            var expectedErrorMessage = "Person Already Has An Account With Type";
            actual.ErrorMessage.Should().Be(expectedErrorMessage);

            await sut.PersonReader.Received().GetPersonByIdAsync(personId);
            await sut.AccountReader.Received()
                    .GetAccountByPersonIdAndAccountTypeAsync(personId, accountType);
        }

        [Theory, AutoNSubstituteData]
        public async Task ValidateAsync_WhenPersonIsValid_ShouldReturnValid(
            int personId,
            AccountTypeEnum accountType,
            Pessoa pessoa,
            AccountCreateValidator sut)
        {
            sut.PersonReader.GetPersonByIdAsync(Arg.Any<int>()).Returns(pessoa);
            sut.AccountReader
                .GetAccountByPersonIdAndAccountTypeAsync(
                    Arg.Any<int>(), Arg.Any<AccountTypeEnum>()).ReturnsNull();

            var actual = await sut.ValidateAsync(personId, accountType);

            actual.IsValid.Should().BeTrue();

            actual.ErrorMessage.Should().BeNull();

            await sut.PersonReader.Received().GetPersonByIdAsync(personId);
            await sut.AccountReader.Received()
                    .GetAccountByPersonIdAndAccountTypeAsync(personId, accountType);
        }
    }
}

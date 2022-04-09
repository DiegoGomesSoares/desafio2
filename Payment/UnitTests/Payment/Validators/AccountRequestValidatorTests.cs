using Domain.Enums;
using Domain.Models.Requests;
using FluentAssertions;
using Payment.Validators;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Validators
{
    public class AccountRequestValidatorTests
    {
        [Theory]
        [AutoInlineData(0)]
        [AutoInlineData(-1)]
        [AutoInlineData(null)]
        public void PersonId_WhenIsNullOrLessThen0_ShouldReturnErrorMessage(
            int personId,
            AccountRequest request)
        {
            var validator = new AccountRequestValidator();
            request.PersonId = personId;

            var result = validator.Validate(request);

            var expectedMessage = "\'Person Id\' must be greater than \'0\'.";

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be(expectedMessage);            
        }

        [Theory, AutoInlineData]
        public void Type_WhenTypeIsInvalid_ShouldReturnErrorMessage(
            AccountRequest request)
        {
            var validator = new AccountRequestValidator();
            request.Type = (AccountTypeEnum)50;

            var result = validator.Validate(request);

            var expectedMessage = "\'Type\' has a range of values which does not include \'50\'.";

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be(expectedMessage);
        }

        [Theory]
        [AutoInlineData(0)]
        [AutoInlineData(-1)]
        public void Type_WhenDailyCashOutLimitIsLessThan0_ShouldReturnErrorMessage(
            decimal dailyLimit,
            AccountRequest request)
        {
            var validator = new AccountRequestValidator();
            request.DailyCashOutLimit = dailyLimit;

            var result = validator.Validate(request);

            var expectedMessage = "\'Daily Cash Out Limit\' must be greater than \'0\'.";

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be(expectedMessage);
        }

        [Theory]
        [AutoInlineData(1, AccountTypeEnum.Poupanca)]
        [AutoInlineData(1, AccountTypeEnum.Salario)]
        public void PersonId_WhenRequestIsValid_ShouldValidateCorrectly(
            decimal validDailyLimit,
            AccountTypeEnum validType)
        {
            var validator = new AccountRequestValidator();

            var request = new AccountRequest()
            {
                PersonId = 1,
                Type = validType,
                DailyCashOutLimit = validDailyLimit
            };

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Count.Should().Be(0);
        }
    }
}

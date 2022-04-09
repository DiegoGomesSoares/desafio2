using Domain.Models.Requests;
using FluentAssertions;
using Payment.Validators;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Validators
{
    public class OperationRequestValidatorTests
    {
        [Theory]
        [AutoInlineData(0)]
        [AutoInlineData(-1)]
        [AutoInlineData(null)]
        public void AccountId_WhenIsNullOrLessThen0_ShouldReturnErrorMessage(
            int accountId,
            OperationRequest request)
        {
            var validator = new OperationRequestValidator();
            request.AccountId = accountId;

            var result = validator.Validate(request);

            var expectedMessage = "\'Account Id\' must be greater than \'0\'.";

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be(expectedMessage);
        }

        [Theory]
        [AutoInlineData(0)]
        [AutoInlineData(null)]
        public void Amount_WhenIsNullOrLessThen0_ShouldReturnErrorMessage(
            decimal amount,
            OperationRequest request)
        {
            var validator = new OperationRequestValidator();
            request.AccountId = 1;
            request.Amount = amount;

            var result = validator.Validate(request);

            var expectedMessage = "\'Amount\' must not be equal to \'0\'.";

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be(expectedMessage);
        }

        [Theory]
        [AutoInlineData(1)]
        [AutoInlineData(-1)]
        public void PersonId_WhenRequestIsValid_ShouldValidateCorrectly(
            decimal amount)
        {
            var validator = new OperationRequestValidator();

            var request = new OperationRequest()
            {
                AccountId = 1,
                Amount = amount
            };

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Count.Should().Be(0);
        }
    }
}

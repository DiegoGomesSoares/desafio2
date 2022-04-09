using Domain.Models;
using Domain.Validators;

namespace Payment.Validators
{
    public class AccountIdValidator : IAccountIdValidator
    {
        private static readonly string _invalidAccountIdErrorMessage = "Invalid AccountId";
       
        public ValidateResultModel Validate(int accountId)
        {
            if (accountId == 0)
                return new ValidateResultModel { ErrorMessage = _invalidAccountIdErrorMessage };

            return new ValidateResultModel { IsValid = true };

        }
    }
}

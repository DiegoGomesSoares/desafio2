using Domain.Models;
using Domain.Repository.Reader;
using Domain.Validators;

namespace Infrastructure.Validators
{
    public class GetBalanceValidator : IGetBalanceValidator
    {
        public IAccountReader AccountReader { get; }

        private static readonly string _invalidAccountIdErrorMessage = "Invalid AccountId";

        public GetBalanceValidator(
            IAccountReader accountReader)
        {
            AccountReader = accountReader ?? throw new System.ArgumentNullException(nameof(accountReader));
        }
        public ValidateResultModel ValidateAsync(int accountId)
        {
            if (accountId == 0)
                return new ValidateResultModel { ErrorMessage = _invalidAccountIdErrorMessage };

            return new ValidateResultModel { IsValid = true };

        }
    }
}

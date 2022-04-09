using Domain.Entities;
using Domain.Models;
using Domain.Validators;

namespace Infrastructure.Validators
{
    public class AccountTransactionValidator : IAccountTransactionValidator
    {
        private static readonly string _accountNotFoundErrorMessage = "Account Not Found";
        private static readonly string _accountIsBlockedErrorMessage = "Account Is Blocked";

        public ValidateResultModel ValidateCashinOperation(Conta account)
        {            
            var baseResult = BaseValidate(account);

            if (baseResult != null)            
                return baseResult;            

            return new ValidateResultModel { IsValid = true };
        }

        private static ValidateResultModel BaseValidate(Conta account)
        {
            if (account == null)
                return new ValidateResultModel { ErrorMessage = _accountNotFoundErrorMessage };

            if (account.FlagAtivo == false)
                return new ValidateResultModel { ErrorMessage = _accountIsBlockedErrorMessage };

            return null;
        }
    }
}

using Domain.Entities;
using Domain.Models;
using Domain.Validators;
using System;

namespace Payment.Validators
{
    public class AccountTransactionValidator : IAccountTransactionValidator
    {
        private static readonly string _accountNotFoundErrorMessage = "Account Not Found";
        private static readonly string _accountIsBlockedErrorMessage = "Account Is Blocked";
        private static readonly string _invalidAmountCashinErrorMessage = "Amount Must be greater then 0";
        private static readonly string _invalidAmountCashoutErrorMessage = "Amount Must be less than 0";
        private static readonly string _invalidAmountCashOutDalyLimitErrorMessage = "Amount Must Not to be greater then CashOutDalyLimit";

        public ValidateResultModel ValidateCashinOperation(Conta account, decimal amount)
        {
            var baseResult = BaseValidate(account);

            if (baseResult != null)
                return baseResult;

            if (IsAnInvalidCashinAmount(amount))
                return new ValidateResultModel { ErrorMessage = _invalidAmountCashinErrorMessage };


            return new ValidateResultModel { IsValid = true };
        }

        public ValidateResultModel ValidateCashOutOperation(Conta account, decimal amount)
        {
            var result = BaseValidate(account);

            if (result != null)
                return result;

            result = ValidateCashOut(account, amount);

            if (result != null)
                return result;

            return new ValidateResultModel { IsValid = true };
        }        

        private static bool IsAnInvalidCashinAmount(decimal amount)
        {
            return amount < 0;
        }

        private static ValidateResultModel BaseValidate(Conta account)
        {
            if (account == null)
                return new ValidateResultModel { ErrorMessage = _accountNotFoundErrorMessage };

            if (account.FlagAtivo == false)
                return new ValidateResultModel { ErrorMessage = _accountIsBlockedErrorMessage };

            return null;
        }

        private static ValidateResultModel ValidateCashOut(Conta account, decimal amount)
        {
            if (account.LimiteSaqueDiario.HasValue && amount > account.LimiteSaqueDiario.Value)
                return new ValidateResultModel { ErrorMessage = _invalidAmountCashOutDalyLimitErrorMessage };

            if (IsAnInvalidCasOutAmount(amount))
                return new ValidateResultModel { ErrorMessage = _invalidAmountCashoutErrorMessage };

            return null;
        }

        private static bool IsAnInvalidCasOutAmount(decimal amount)
        {
            return amount > 0;
        }
    }
}

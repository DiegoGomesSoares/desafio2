using Domain.Entities;
using Domain.Models;
using Domain.Validators;
using System;

namespace Payment.ValidationHandlers
{
    public class ValidationHandler : IValidationHandler
    {
        public IAccountTransactionValidator AccountTransactionValidator { get; }

        public ValidationHandler(
            IAccountTransactionValidator accountTransactionValidator)
        {
            AccountTransactionValidator = accountTransactionValidator ?? throw new ArgumentNullException(nameof(accountTransactionValidator));
        }

        public ValidateResultModel Handle(Conta account, decimal amount)
        {
            if (IsCachinOperation(amount))
                return AccountTransactionValidator.ValidateCashinOperation(account, amount);

            return AccountTransactionValidator.ValidateCashOutOperation(account, amount);
        }

        private static bool IsCachinOperation(decimal amount)
        {
            return amount > 0;
        }
    }
}

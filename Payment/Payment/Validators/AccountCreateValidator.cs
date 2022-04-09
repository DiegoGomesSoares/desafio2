using Domain.Enums;
using Domain.Models;
using Domain.Repository.Reader;
using Domain.Validators;
using System;
using System.Threading.Tasks;

namespace Payment.Validators
{
    public class AccountCreateValidator : IAccountCreateValidator
    {
        public IPersonReader PersonReader { get; }
        public IAccountReader AccountReader { get; }

        private static readonly string _invalidPersonErrorMessage = "Invalid Person";
        private static readonly string _invalidAccountTypeErrorMessage = "Person Already Has An Account With Type";

        public AccountCreateValidator(
            IPersonReader personReader,
            IAccountReader accountReader)
        {
            PersonReader = personReader ?? throw new ArgumentNullException(nameof(personReader));
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));
        }

        public async Task<ValidateResultModel> ValidateAsync(int personId, AccountTypeEnum accountType)
        {      
                var person = await PersonReader.GetPersonByIdAsync(personId);

                if (person == null)
                    return new ValidateResultModel { ErrorMessage = _invalidPersonErrorMessage };

                if (await PersonHasAnAccountWithSameType(personId, accountType))
                    return new ValidateResultModel { ErrorMessage = _invalidAccountTypeErrorMessage };

                return new ValidateResultModel { IsValid = true };
        }

        private async Task<bool> PersonHasAnAccountWithSameType(int personId, AccountTypeEnum accountType)
        {
            var account = await AccountReader.GetAccountByPersonIdAndAccountTypeAsync(personId, accountType);

            return account != null;
        }
    }
 }

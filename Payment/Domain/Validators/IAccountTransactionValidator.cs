using Domain.Entities;
using Domain.Models;

namespace Domain.Validators
{
    public interface IAccountTransactionValidator
    {
        ValidateResultModel ValidateCashinOperation(Conta account);
    }
}

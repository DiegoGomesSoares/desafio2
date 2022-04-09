using Domain.Models;

namespace Domain.Validators
{
    public interface IAccountIdValidator
    {
        ValidateResultModel Validate(int accountId);
    }
}

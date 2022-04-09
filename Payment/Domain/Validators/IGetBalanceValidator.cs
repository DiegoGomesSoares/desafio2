using Domain.Models;

namespace Domain.Validators
{
    public interface IGetBalanceValidator
    {
        ValidateResultModel ValidateAsync(int accountId);
    }
}

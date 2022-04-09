using Domain.Enums;
using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public interface IAccountCreateValidator
    {
        Task<ValidateResultModel> ValidateAsync(int personId, AccountTypeEnum accountType);
    }
}

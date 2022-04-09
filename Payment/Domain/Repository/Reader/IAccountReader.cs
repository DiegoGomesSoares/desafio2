using Domain.Entities;
using Domain.Enums;
using System.Threading.Tasks;

namespace Domain.Repository.Reader
{
    public interface IAccountReader
    {
        Task<Conta> GetAccountByPersonIdAndAccountTypeAsync(int personId, AccountTypeEnum accountType);
        Task<Conta> GetAccountByIdAsync(int accountId);
    }
}

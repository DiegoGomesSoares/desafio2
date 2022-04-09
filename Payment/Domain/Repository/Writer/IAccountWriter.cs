using Domain.Entities;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Domain.Repository.Writer
{
    public interface IAccountWriter
    {
        Task<int> CreateAsync(Conta account);
        Task UpdateBalanceAsync(int accountId, decimal newBalanceValue, SqlCommand command);
        Task BlockAccount(int accountId);
    }
}

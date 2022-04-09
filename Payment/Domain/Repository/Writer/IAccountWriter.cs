using Domain.Entities;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Domain.Repository.Writer
{
    public interface IAccountWriter
    {
        Task<int> CreateAsync(Conta conta);
        Task UpdateBalanceAsync(int idConta, decimal newBalanceValue, SqlCommand command);
    }
}

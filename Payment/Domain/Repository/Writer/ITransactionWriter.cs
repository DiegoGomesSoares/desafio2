using Domain.Entities;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Domain.Repository.Writer
{
    public interface ITransactionWriter
    {
        Task<int> CreateAsync(Transacao transaction, SqlCommand command);
    }
}

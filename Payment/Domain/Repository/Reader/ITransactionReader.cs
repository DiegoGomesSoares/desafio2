using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repository.Reader
{
    public interface ITransactionReader
    {
        Task<int> GetTotalCountAsync(int accountId, string starDate, string endDate);
        Task<IEnumerable<Transacao>> GetAllPaginatedAsync(
                    int accountId, string starDate, 
                    string endDate, int pageNumber, int pageSize);
    }
}

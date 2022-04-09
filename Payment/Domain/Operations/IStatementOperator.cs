using Domain.Models.Requests;
using System.Threading.Tasks;

namespace Domain.Operations
{
    public interface IStatementOperator
    {
        Task<StatementResponse> GetStatementAsync(StatementRequest model);
    }
}

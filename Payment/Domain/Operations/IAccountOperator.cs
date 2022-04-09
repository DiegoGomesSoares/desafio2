using Domain.Entities;
using Domain.Models.Responses;
using System.Threading.Tasks;

namespace Domain.Operations
{
    public interface IAccountOperator
    {
        Task<OperationResponse> ExecuteOperationAsync(Conta conta, decimal amount);
    }
}

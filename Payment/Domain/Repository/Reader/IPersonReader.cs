using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Repository.Reader
{
    public interface IPersonReader
    {
        Task<Pessoa> GetPersonByIdAsync(int personId);
    }
}

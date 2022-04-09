using Dapper;
using Domain.Entities;
using Domain.Repository;
using Domain.Repository.Reader;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Reader
{
    public class PersonReader : IPersonReader
    {
        public IConnectionFactory ConnectionFactory { get; }

        public PersonReader(
            IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }
        public async Task<Pessoa> GetPersonByIdAsync(int personId)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                var sqlQuery =
@"SELECT * 
FROM [Pessoas] p (NOLOCK)
WHERE p.idPessoa = @personId";

                var result = await connection.QueryFirstOrDefaultAsync<Pessoa>(sqlQuery, new { personId });

                return result;
            }
        }
    }
}

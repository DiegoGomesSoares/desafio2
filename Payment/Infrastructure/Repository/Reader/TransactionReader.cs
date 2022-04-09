using Dapper;
using Domain.Entities;
using Domain.Repository;
using Domain.Repository.Reader;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Reader
{
    public class TransactionReader : ITransactionReader
    {
        public IConnectionFactory ConnectionFactory { get; }

        public TransactionReader(
            IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<int> GetTotalCountAsync(int accountId, string starDate, string endDate)
        {
            using var connection = ConnectionFactory.CreateConnection();
            var sqlQuery =
@"SELECT
    COUNT(*)
FROM
    [Transacoes] t (NOLOCK)
WHERE
    t.idConta = @accountId and t.[dataTransacao] >= @starDate and t.[dataTransacao] <= @endDate";

            var result = await connection.QueryFirstAsync<int>(sqlQuery, new { accountId, starDate, endDate });

            return result;
        }

        public async Task<IEnumerable<Transacao>> GetAllPaginatedAsync(
                int accountId, string starDate, string endDate, int pageNumber, int pageSize)
        {
            using var connection = ConnectionFactory.CreateConnection();
            var sqlQuery =
@"SELECT
    *
FROM
    [Transacoes] t (NOLOCK)
WHERE
    t.idConta = @accountId and t.[dataTransacao] >= @starDate and t.[dataTransacao] <= @endDate
ORDER BY t.dataTransacao DESC
OFFSET @pageSize * (@pageNumber - 1) ROWS
FETCH NEXT @pageSize ROWS ONLY";

            var result = await connection
                            .QueryAsync<Transacao>(sqlQuery, 
                                new { accountId, starDate, endDate, pageNumber, pageSize });

            return result;
        }
    }
}

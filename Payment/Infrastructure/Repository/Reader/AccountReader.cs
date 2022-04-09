using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using Domain.Repository.Reader;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Reader
{
    public class AccountReader : IAccountReader
    {
        public IConnectionFactory ConnectionFactory { get; }

        public AccountReader(
            IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory ?? throw new System.ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<Conta> GetAccountByPersonIdAndAccountTypeAsync(int personId, AccountTypeEnum accountType)
        {
            using var connection = ConnectionFactory.CreateConnection();
            var sqlQuery =
@"SELECT * 
FROM [Contas] c (NOLOCK)
WHERE c.idPessoa = @personId and c.tipoConta = @accountType";

            var result = await connection.QueryFirstOrDefaultAsync<Conta>(sqlQuery, new { personId, accountType });

            return result;
        }

        public async Task<Conta> GetAccountByIdAsync(int accountId)
        {
            using var connection = ConnectionFactory.CreateConnection();
            var sqlQuery =
@"SELECT * 
FROM [Contas] c (NOLOCK)
WHERE c.idConta = @accountId";

            var result = await connection.QueryFirstOrDefaultAsync<Conta>(sqlQuery, new { accountId });

            return result;
        }
    }
}

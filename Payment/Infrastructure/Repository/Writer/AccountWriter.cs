using Dapper;
using Domain.Entities;
using Domain.Repository;
using Domain.Repository.Writer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Writer
{
    public class AccountWriter : IAccountWriter
    {
        public IConnectionFactory ConnectionFactory { get; }

        public AccountWriter(
            IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<int> CreateAsync(Conta account)
        {
            using var connection = ConnectionFactory.CreateConnection();
            var query =
@"INSERT INTO [dbo].[Contas]
    ([idPessoa], [saldo] ,[limiteSaqueDiario], [flagAtivo], [tipoConta], [dataCriacao])
OUTPUT 
    INSERTED.idConta
VALUES
    (@IdPessoa, @Saldo, @LimiteSaqueDiario, @FlagAtivo, @TipoConta, @DataCriacao)";

            await connection.OpenAsync();
            return await connection.ExecuteScalarAsync<int>(query, account);
        }

        public async Task UpdateBalanceAsync(int accountId, decimal newBalanceValue, SqlCommand command)
        {
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText =
@"UPDATE [dbo].[Contas] 
SET 
    [saldo] = @newBalanceValue   
WHERE 
    [idConta] = @accountId";

            command.Parameters.Add("@newBalanceValue", SqlDbType.Money).Value = newBalanceValue;
            command.Parameters.Add("@accountId", SqlDbType.Int).Value = accountId;

            await command.ExecuteNonQueryAsync();
        }

        public async Task BlockAccount(int accountId)
        {
            using var connection = ConnectionFactory.CreateConnection();
            var sqlQuery =
@"UPDATE[dbo].[Contas]
SET
    [flagAtivo] = 0
WHERE
    [idConta] = @accountId";

            await connection.OpenAsync();
            await connection.ExecuteAsync(sqlQuery, new { accountId });
        }
    }
}

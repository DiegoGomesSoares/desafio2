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

        public async Task<int> CreateAsync(Conta conta)
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
            return await connection.ExecuteScalarAsync<int>(query, conta);
        }

        public async Task UpdateBalanceAsync(int idConta, decimal newBalanceValue, SqlCommand command)
        {
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText =
@"UPDATE [dbo].[Contas] 
SET 
    [saldo] = @newBalanceValue   
WHERE 
    [idConta] = @idConta";

            command.Parameters.Add("@newBalanceValue", SqlDbType.Money).Value = newBalanceValue;
            command.Parameters.Add("@idConta", SqlDbType.Int).Value = idConta;

            await command.ExecuteNonQueryAsync();
        }
    }
}

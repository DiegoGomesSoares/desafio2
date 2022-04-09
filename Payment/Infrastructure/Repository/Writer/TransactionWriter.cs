using Domain.Entities;
using Domain.Repository.Writer;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Writer
{
    public class TransactionWriter : ITransactionWriter
    {
        public async Task<int> CreateAsync(Transacao transaction, SqlCommand command)
        {
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText =
@"INSERT INTO [dbo].[Transacoes]
    ([idConta], [valor], [dataTransacao])
OUTPUT 
    INSERTED.idTransacao
VALUES
    (@idConta, @valor, @dataTransacao)";

            command.Parameters.Add("@idConta", SqlDbType.Int).Value = transaction.IdConta;
            command.Parameters.Add("@valor", SqlDbType.Money).Value = transaction.Valor;
            command.Parameters.Add("@dataTransacao", SqlDbType.DateTime).Value = transaction.DataTransacao;

            return (int) await command.ExecuteScalarAsync();
        }
    }
}

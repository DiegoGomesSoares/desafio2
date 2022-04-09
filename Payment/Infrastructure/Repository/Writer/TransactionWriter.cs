using Domain.Entities;
using Domain.Repository.Writer;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Writer
{
    public class TransactionWriter : ITransactionWriter
    {
        public async Task<int> CreateAsync(Transacao transacao, SqlCommand command)
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

            command.Parameters.Add("@idConta", SqlDbType.Int).Value = transacao.IdConta;
            command.Parameters.Add("@valor", SqlDbType.Money).Value = transacao.Valor;
            command.Parameters.Add("@dataTransacao", SqlDbType.VarChar, 50).Value = transacao.DataTransacao;

            return (int) await command.ExecuteScalarAsync();
        }
    }
}

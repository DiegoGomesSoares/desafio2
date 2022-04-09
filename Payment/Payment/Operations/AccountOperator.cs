using Domain.Entities;
using Domain.Models.Responses;
using Domain.Operations;
using Domain.Repository;
using Domain.Repository.Writer;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Payment.Operations
{
    public class AccountOperator : IAccountOperator
    {
        public IAccountWriter AccountWriter { get; }
        public ITransactionWriter TransactionWriter { get; }
        public IConnectionFactory ConnectionFactory { get; }

        public AccountOperator(
            IAccountWriter accountWriter,
            ITransactionWriter transactionWriter,
            IConnectionFactory connectionFactory)
        {
            AccountWriter = accountWriter ?? throw new ArgumentNullException(nameof(accountWriter));
            TransactionWriter = transactionWriter ?? throw new ArgumentNullException(nameof(transactionWriter));
            ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<OperationResponse> ExecuteOperationAsync(Conta conta, decimal amount)
        {
            var newBalanceValue = conta.Saldo + amount;

            using var connection = ConnectionFactory.CreateConnection();
            using var command = connection.CreateCommand();
            await connection.OpenAsync();
            SqlTransaction sqlTransaction = connection.BeginTransaction();
            command.Transaction = sqlTransaction;

            try
            {
                var transaction = CreateTransactionModel(conta.IdConta, amount);

                transaction.IdTransacao = await TransactionWriter.CreateAsync(transaction, command);

                await AccountWriter.UpdateBalanceAsync(conta.IdConta, newBalanceValue, command);

                sqlTransaction.Commit();

                return CreateResponse(transaction, conta.IdConta, newBalanceValue);
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();

                throw;
            }
        }

        private static OperationResponse CreateResponse(Transacao transaction, int idConta, decimal newBalanceValue)
        {
            var response = new OperationResponse
            {
                IdConta = idConta,
                Saldo = newBalanceValue,
                Transacao = new TransactionResponse
                {
                    IdTransacao = transaction.IdTransacao,
                    Valor = transaction.Valor,
                    DataCriacao = transaction.DataTransacao
                }
            };
            return response;
        }

        private static Transacao CreateTransactionModel(int contaId, decimal amount)
        {
            return new Transacao
            {
                IdConta = contaId,
                Valor = amount,
                DataTransacao = DateTime.Now
            };
        }
    }
}

using Domain.Entities;
using Domain.Models.Requests;
using Domain.Models.Responses;
using System.Collections.Generic;
using System.Linq;

namespace Payment
{
    public static class StatementResponseHelpers
    {
        public static StatementResponse MapTransactions(IEnumerable<Transacao> transactions)
        {
            var statement = new StatementResponse();
            statement.Transacoes = transactions.Select(transaction => 
                new TransactionResponse
                {
                    IdTransacao = transaction.IdTransacao,
                    Valor = transaction.Valor,
                    DataCriacao = transaction.DataTransacao
                }).ToList();

            return statement;
        }
    }
}

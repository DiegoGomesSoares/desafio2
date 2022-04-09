using System;

namespace Domain.Models.Responses
{
    public class TransactionResponse
    {
        public int IdTransacao { get; set; }
        public Decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }

    }
}

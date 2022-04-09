using System;

namespace Domain.Models.Responses
{
    public class OperationResponse
    {       
        public int IdConta { get; set; }
        public Decimal Saldo { get; set; }
        public TransactionResponse Transacao { get; set; }
    }
}

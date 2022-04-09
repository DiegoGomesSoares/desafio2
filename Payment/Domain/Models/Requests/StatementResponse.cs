using Domain.Models.Responses;
using System.Collections.Generic;

namespace Domain.Models.Requests
{
    public class StatementResponse
    {
        public int TotalTransacao { get; set; }

        public int TotalPaginas { get; set; }

        public int PaginaIndex { get; set; }

        public int IdConta { get; set; }

        public decimal Saldo { get; set; }

        public IEnumerable<TransactionResponse> Transacoes { get; set; }
    }
}

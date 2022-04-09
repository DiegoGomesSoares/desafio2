using System;

namespace Domain.Models.Responses
{
    public class AccountResponse
    {
        public int IdConta { get; set; }       
        public decimal Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}

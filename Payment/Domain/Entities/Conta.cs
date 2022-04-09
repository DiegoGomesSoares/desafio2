using System;

namespace Domain.Entities
{
    public class Conta
    {
        public int IdConta { get; set; }
        public int IdPessoa { get; set; }
        public decimal Saldo { get; set; }
        public decimal? LimiteSaqueDiario { get; set; }
        public bool FlagAtivo { get; set; }
        public short TipoConta { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}

using System;

namespace Domain.Entities
{
    public class Transacao
    {
        public int IdTransacao { get; set; }
        public int IdConta { get; set; }
        public Decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; }

    }
}

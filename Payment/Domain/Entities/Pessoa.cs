using System;

namespace Domain.Entities
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public int IdConta { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
{
    public class Fornecedor:IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Nome { get; protected set; }
        public virtual string Email { get; protected set; }

        protected Fornecedor() { }

        public Fornecedor(string codigo, string nome, string email)
        {
            Codigo = codigo;
            Nome = nome;
            Email = email;
        }

        public virtual void Atualizar(string novoNome, string novoEmail)
        {
            Nome = novoNome;
            Email = novoEmail;
        }
    }
}

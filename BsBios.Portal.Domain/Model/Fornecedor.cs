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

        protected Fornecedor() { }

        public Fornecedor(string codigo, string nome)
        {
            Codigo = codigo;
            Nome = nome;
        }
    }
}

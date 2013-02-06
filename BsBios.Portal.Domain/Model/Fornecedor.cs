using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
{
    public class Fornecedor:IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual string CodigoSap { get; protected set; }
        public virtual string Nome { get; protected set; }

        protected Fornecedor() { }

        public Fornecedor(string codigoSap, string nome)
        {
            CodigoSap = codigoSap;
            Nome = nome;
        }
    }
}

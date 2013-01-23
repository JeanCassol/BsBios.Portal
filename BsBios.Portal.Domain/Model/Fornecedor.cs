using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
{
    public class Fornecedor
    {
        protected Fornecedor(){}
        public virtual int Id { get; protected set; }
        public virtual string Nome { get; protected set; }
    }
}

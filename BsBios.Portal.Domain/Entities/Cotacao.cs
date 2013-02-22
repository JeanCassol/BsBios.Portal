using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Entities
{
    public class Cotacao
    {
        public virtual ProcessoDeCotacao ProcessoDeCotacao { get; protected set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual decimal? ValorUnitario { get; set; }
        public virtual decimal? QuantidadeAdquirida { get; set; }

        protected Cotacao(){}

        public Cotacao(ProcessoDeCotacao processoDeCotacao, Fornecedor fornecedor)
        {
            ProcessoDeCotacao = processoDeCotacao;
            Fornecedor = fornecedor;
            //QuantidadeAdquirida = 0;
        }
    }

   
}

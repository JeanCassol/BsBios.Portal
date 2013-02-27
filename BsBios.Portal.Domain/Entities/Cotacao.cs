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
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual decimal? ValorUnitario { get; protected set; }
        public virtual decimal? QuantidadeAdquirida { get; protected set; }
        public virtual Incoterm Incoterm { get; protected set; }
        public virtual string DescricaoIncoterm{ get; protected set; }

        protected Cotacao(){}

        public Cotacao(ProcessoDeCotacao processoDeCotacao, Fornecedor fornecedor)
        {
            ProcessoDeCotacao = processoDeCotacao;
            Fornecedor = fornecedor;
        }

        public virtual void Atualizar(decimal valorUnitario, decimal quantidadeAdquirida, Incoterm incoterm, string descricaoIncoterm)
        {
            ValorUnitario = valorUnitario;
            QuantidadeAdquirida = quantidadeAdquirida;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;

        }

    }

   
}

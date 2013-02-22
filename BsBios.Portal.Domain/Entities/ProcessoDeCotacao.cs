using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class ProcessoDeCotacao : IAggregateRoot
    {
        public virtual Enumeradores.StatusPedidoCotacao Status { get; protected set; }
        public virtual DateTime? DataLimiteDeRetorno { get; protected set; }
        public virtual IList<Fornecedor> Fornecedores { get; protected set; }

        protected ProcessoDeCotacao()
        {
            Fornecedores = new List<Fornecedor>();
            Status = Enumeradores.StatusPedidoCotacao.NaoIniciado;
        }
    }

    public class ProcessoDeCotacaoDeMaterial: ProcessoDeCotacao
    {
        public virtual RequisicaoDeCompra RequisicaoDeCompra { get; protected set; }

        public ProcessoDeCotacaoDeMaterial(RequisicaoDeCompra requisicaoDeCompra)
        {
            RequisicaoDeCompra = requisicaoDeCompra;
        }
    }
}

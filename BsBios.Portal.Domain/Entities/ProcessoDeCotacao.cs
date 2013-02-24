using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class ProcessoDeCotacao : IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual Enumeradores.StatusPedidoCotacao Status { get; protected set; }
        public virtual Produto Produto { get; protected set; }
        public virtual decimal Quantidade { get; protected set; }
        public virtual DateTime? DataLimiteDeRetorno { get; protected set; }
        public virtual IList<Fornecedor> Fornecedores { get; protected set; }

        protected ProcessoDeCotacao()
        {
            Fornecedores = new List<Fornecedor>();
            Status = Enumeradores.StatusPedidoCotacao.NaoIniciado;
        }

        protected ProcessoDeCotacao(Produto produto, decimal quantidade):this()
        {
            Produto = produto;
            Quantidade = quantidade;
        }
    }

    public class ProcessoDeCotacaoDeMaterial: ProcessoDeCotacao
    {
        public virtual RequisicaoDeCompra RequisicaoDeCompra { get; protected set; }

        protected ProcessoDeCotacaoDeMaterial(){}
        public ProcessoDeCotacaoDeMaterial(RequisicaoDeCompra requisicaoDeCompra, Produto produto, decimal quantidade):base(produto, quantidade)
        {
            RequisicaoDeCompra = requisicaoDeCompra;
            Produto = produto;
            Quantidade = quantidade;
        }
    }
}

using System.Collections.Generic;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class ProcessoDeCotacaoItem
    {
        public virtual int Id { get; protected set; }
        public virtual ProcessoDeCotacao ProcessoDeCotacao { get; protected set; }
        public virtual Produto Produto { get; protected set; }
        public virtual decimal Quantidade { get; protected set; }
        public virtual UnidadeDeMedida UnidadeDeMedida { get; protected set; }
        public virtual IList<CotacaoItem> Cotacoes { get; protected set; }

        protected ProcessoDeCotacaoItem(){}

        protected ProcessoDeCotacaoItem(ProcessoDeCotacao processoDeCotacao, Produto produto, decimal quantidade
            , UnidadeDeMedida unidadeDeMedida)
        {
            ProcessoDeCotacao = processoDeCotacao;
            Produto = produto;
            Quantidade = quantidade;
            UnidadeDeMedida = unidadeDeMedida;
        }

        public virtual bool SuperouQuantidadeSolicitada(decimal quantidadeTotalAdquirida)
        {
            return quantidadeTotalAdquirida > Quantidade;
        }

    }

    public class ProcessoDeCotacaoDeMaterialItem: ProcessoDeCotacaoItem
    {
        public virtual RequisicaoDeCompra RequisicaoDeCompra { get; protected set; }
        protected ProcessoDeCotacaoDeMaterialItem(){}
        internal ProcessoDeCotacaoDeMaterialItem(ProcessoDeCotacao processoDeCotacao,
                                               RequisicaoDeCompra requisicaoDeCompra)
            : base(processoDeCotacao, requisicaoDeCompra.Material, requisicaoDeCompra.Quantidade,
                requisicaoDeCompra.UnidadeMedida)
        {
            RequisicaoDeCompra = requisicaoDeCompra;
            requisicaoDeCompra.VincularComProcessoDeCotacao();
        }
    }
    public class ProcessoDeCotacaoDeFreteItem : ProcessoDeCotacaoItem
    {
        protected ProcessoDeCotacaoDeFreteItem(){}
        internal ProcessoDeCotacaoDeFreteItem(ProcessoDeCotacao processoDeCotacao, Produto material, decimal quantidade
            , UnidadeDeMedida unidadeDeMedida):base(processoDeCotacao, material, quantidade, unidadeDeMedida) { }

        protected internal virtual void Atualizar(Produto produto, decimal quantidadeMaterial, UnidadeDeMedida unidadeDeMedida)
        {
            Produto = produto;
            Quantidade = quantidadeMaterial;
            UnidadeDeMedida = unidadeDeMedida;
        }

    }
}

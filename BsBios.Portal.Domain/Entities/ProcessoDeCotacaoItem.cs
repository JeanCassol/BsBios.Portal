using System.Collections.Generic;
using BsBios.Portal.Common;

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

        protected void Atualizar(Produto produto, decimal quantidadeMaterial, UnidadeDeMedida unidadeDeMedida)
        {
            Produto = produto;
            Quantidade = quantidadeMaterial;
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

        public virtual decimal Cadencia { get; protected set; }
        public virtual decimal? ValorPrevisto { get; protected set; }
        public virtual Enumeradores.TipoDePrecoDoProcessoDeCotacao TipoDePreco { get; protected set; }
        public virtual decimal? ValorFechado { get; protected set; }
        public virtual decimal? ValorMaximo { get; protected set; }

        protected ProcessoDeCotacaoDeFreteItem(){}

        internal ProcessoDeCotacaoDeFreteItem(ProcessoDeCotacao processoDeCotacao, Produto material, decimal quantidade
            , UnidadeDeMedida unidadeDeMedida, decimal cadencia, decimal? valorPrevisto) : base(processoDeCotacao, material, quantidade, unidadeDeMedida)
        {
            this.Cadencia = cadencia;
            this.ValorPrevisto = valorPrevisto;
        }

        protected internal virtual void Atualizar(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida, decimal cadencia, decimal? valorPrevisto)
        {
            base.Atualizar(produto, quantidade, unidadeDeMedida);
            this.Cadencia = cadencia;
            this.ValorPrevisto = valorPrevisto;

        }

        public virtual void AbrirPreco()
        {
            TipoDePreco = Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorAberto;
            ValorFechado = null;
            ValorMaximo = null;
        }

        public virtual void FecharPreco(decimal preco)
        {
            TipoDePreco = Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorFechado;
            ValorFechado = preco;
            ValorMaximo = null;
        }

        public virtual void EstabelecerPrecoMaximo(decimal preco)
        {
            TipoDePreco = Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorMaximo;
            ValorFechado = null;
            ValorMaximo = preco;
        }


    }
}

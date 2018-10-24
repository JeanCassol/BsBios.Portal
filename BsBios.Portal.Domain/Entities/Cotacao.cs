using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class Cotacao
    {
        public virtual int Id { get; protected set; }
        public virtual IList<CotacaoItem> Itens { get; protected set; }

        protected Cotacao()
        {
            Itens = new List<CotacaoItem>();
        }

        public virtual bool Selecionada => this.Itens.Any(x => x.Selecionada);

        public abstract void RemoverValores();
    }

    public class CotacaoDeFrete : Cotacao
    {
        public virtual CotacaoItem InformarCotacaoDeItem(ProcessoDeCotacaoItem processoDeCotacaoItem,
            decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes)
        {
            var cotacaoItem =
                (CotacaoFreteItem) Itens.SingleOrDefault(item =>
                    item.ProcessoDeCotacaoItem.Id == processoDeCotacaoItem.Id);
            if (cotacaoItem != null)
            {
                if (cotacaoItem.ValorComImpostos > 0)
                {
                    throw new AlterarCotacaoDeFreteException();
                }
                cotacaoItem.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);
            }
            else
            {
                cotacaoItem = new CotacaoFreteItem(this, processoDeCotacaoItem, valorTotalComImpostos,
                    quantidadeDisponivel, observacoes);
                Itens.Add(cotacaoItem);
            }

            return cotacaoItem;
        }

        public virtual string NumeroDaCondicaoGeradaNoSap { get; protected set; }

        public virtual void Selecionar(decimal quantidadeAdquirida, decimal cadencia)
        {
            var itemDaCotacao = (CotacaoFreteItem) this.Itens.Single();
            itemDaCotacao.Selecionar(quantidadeAdquirida, cadencia);
        }

        public virtual void RemoverSelecao()
        {
            var itemDaCotacao = (CotacaoFreteItem) this.Itens.Single();
            itemDaCotacao.RemoverSelecao();
        }

        protected internal virtual void InformarNumeroDaCondicao(string numeroGeradoNoSap)
        {
            NumeroDaCondicaoGeradaNoSap = numeroGeradoNoSap;
        }

        public override void RemoverValores()
        {
            var itemDaCotacao = this.Itens.Single();
            itemDaCotacao.RemoverValores();
        }
    }

    public class CotacaoMaterial : Cotacao
    {
        public virtual CondicaoDePagamento CondicaoDePagamento { get; protected set; }
        public virtual Incoterm Incoterm { get; protected set; }
        public virtual string DescricaoIncoterm { get; protected set; }

        protected CotacaoMaterial()
        {
        }

        internal CotacaoMaterial(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoIncoterm)
        {
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
        }

        public virtual void Atualizar(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm,
            string descricaoIncoterm)
        {
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
        }

        public virtual CotacaoItem InformarCotacaoDeItem(ProcessoDeCotacaoItem processoDeCotacaoItem, decimal preco,
            decimal? mva,
            decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            var cotacaoItem =
                (CotacaoMaterialItem) Itens.SingleOrDefault(item =>
                    item.ProcessoDeCotacaoItem.Id == processoDeCotacaoItem.Id);
            if (cotacaoItem != null)
            {
                cotacaoItem.Atualizar(preco, mva, quantidadeDisponivel, prazoDeEntrega, observacoes);
            }
            else
            {
                cotacaoItem = new CotacaoMaterialItem(this, processoDeCotacaoItem, mva, prazoDeEntrega, preco,
                    quantidadeDisponivel, observacoes);
                Itens.Add(cotacaoItem);
            }

            return cotacaoItem;
        }

        public override void RemoverValores()
        {
            throw new NotImplementedException();
        }
    }
}
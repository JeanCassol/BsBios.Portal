using System;
using System.Collections.Generic;
using System.Linq;

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
    }

  /*      protected Cotacao(decimal valorTotalComImpostos, decimal quantidadeDisponivel,string observacoes):this()
        {
            ValorComImpostos = valorTotalComImpostos;
            QuantidadeDisponivel = quantidadeDisponivel;
            Observacoes = observacoes;
            CalculaValorLiquido();
        }
        
        private decimal ValorDoImposto(Enumeradores.TipoDeImposto tipoDeImposto)
        {
            var imposto = Imposto(tipoDeImposto);
            return imposto != null ? imposto.Valor : 0;
        }

        private void CalculaValorLiquido()
        {
            
            ValorLiquido = ValorComImpostos - ValorDoImposto(Enumeradores.TipoDeImposto.Icms)
                - ValorDoImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao)
                - ValorDoImposto(Enumeradores.TipoDeImposto.Ipi);
        }

        protected virtual void Atualizar(decimal valorTotalComImpostos,decimal quantidadeDisponivel, string observacoes)
        {
            ValorComImpostos = valorTotalComImpostos;
            QuantidadeDisponivel = quantidadeDisponivel;
            Observacoes = observacoes;
            CalculaValorLiquido();
        }*/


        //private void AtualizarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal aliquota, decimal valor)
        //{
        //    var cotacaoItem = (CotacaoFreteItem)Itens.SingleOrDefault(item => item.ProcessoDeCotacaoItem.Id == processoDeCotacaoItem.Id);
        //    if (cotacaoItem != null)
        //    {
        //        cotacaoItem.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);
        //    }
        //    else
        //    {
        //        cotacaoItem = new CotacaoFreteItem(this, processoDeCotacaoItem, valorTotalComImpostos, quantidadeDisponivel, observacoes);
        //        Itens.Add(cotacaoItem);
        //    }

        //    return cotacaoItem;
        //}


        //public virtual Imposto Imposto(Enumeradores.TipoDeImposto tipo)
        //{
        //    return Impostos.SingleOrDefault(x => x.Tipo == tipo);
        //}

        //protected virtual void Selecionar(decimal quantidadeAdquirida)
        //{
        //    Selecionada = true;
        //    QuantidadeAdquirida = quantidadeAdquirida;
        //}

        //protected virtual void RemoverSelecao()
        //{
        //    Selecionada = false;
        //    QuantidadeAdquirida = null;
        //}

    

    public class CotacaoDeFrete: Cotacao
    {
        //public CotacaoDeFrete(decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes)
        //    : base(valorTotalComImpostos, quantidadeDisponivel, observacoes){}

        public virtual CotacaoItem InformarCotacaoDeItem(ProcessoDeCotacaoItem processoDeCotacaoItem, decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes)
        {
            var cotacaoItem = (CotacaoFreteItem)Itens.SingleOrDefault(item => item.ProcessoDeCotacaoItem.Id == processoDeCotacaoItem.Id);
            if (cotacaoItem != null)
            {
                cotacaoItem.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);
            }
            else
            {
                cotacaoItem = new CotacaoFreteItem(this, processoDeCotacaoItem, valorTotalComImpostos, quantidadeDisponivel, observacoes);
                Itens.Add(cotacaoItem);
            }

            return cotacaoItem;
        }


        public virtual decimal? Cadencia { get; protected set; }
        public virtual string NumeroDaCondicaoGeradaNoSap { get; protected set; }


        //public new virtual void Atualizar(decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes)
        //{
        //    base.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);
        //}


        public virtual void Selecionar(decimal quantidadeAdquirida, decimal cadencia)
        {
            Cadencia = cadencia;
            var itemDaCotacao = this.Itens.Single();
            itemDaCotacao.Selecionar(quantidadeAdquirida);

        }

        public virtual void RemoverSelecao()
        {
            Cadencia = null;
            var itemDaCotacao = this.Itens.Single();
            itemDaCotacao.RemoverSelecao();

        }

        protected internal virtual void InformarNumeroDaCondicao(string numeroGeradoNoSap)
        {
            NumeroDaCondicaoGeradaNoSap = numeroGeradoNoSap;
        }

    }

    public class CotacaoMaterial: Cotacao
    {
        public virtual CondicaoDePagamento CondicaoDePagamento { get; protected set; }
        public virtual Incoterm Incoterm { get; protected set; }
        public virtual string DescricaoIncoterm { get; protected set; }

        protected CotacaoMaterial(){}

        internal CotacaoMaterial(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoIncoterm)
        {
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
        }

        public virtual void Atualizar(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoIncoterm)
        {
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
        }

        public virtual CotacaoItem InformarCotacaoDeItem(ProcessoDeCotacaoItem processoDeCotacaoItem, decimal preco, decimal? mva, 
            decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            var cotacaoItem = (CotacaoMaterialItem)Itens.SingleOrDefault(item => item.ProcessoDeCotacaoItem.Id == processoDeCotacaoItem.Id);
            if (cotacaoItem != null)
            {
                cotacaoItem.Atualizar(preco, mva, quantidadeDisponivel, prazoDeEntrega, observacoes);
            }
            else
            {
                cotacaoItem = new CotacaoMaterialItem(this, processoDeCotacaoItem, mva, prazoDeEntrega, preco, quantidadeDisponivel, observacoes);
                Itens.Add(cotacaoItem);
            }

            return cotacaoItem;
        }

    }

   
}

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

    }

    public class CotacaoFrete: Cotacao
    {
        //protected CotacaoFrete(){}
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

        public virtual CotacaoItem InformarCotacaoDeItem(ProcessoDeCotacaoItem processoDeCotacaoItem, decimal valorTotalComImpostos, decimal? mva, 
            decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            var cotacaoItem = (CotacaoMaterialItem)Itens.SingleOrDefault(item => item.ProcessoDeCotacaoItem.Id == processoDeCotacaoItem.Id);
            if (cotacaoItem != null)
            {
                cotacaoItem.Atualizar(valorTotalComImpostos, mva, quantidadeDisponivel, prazoDeEntrega, observacoes);
            }
            else
            {
                cotacaoItem = new CotacaoMaterialItem(this, processoDeCotacaoItem, mva, prazoDeEntrega, valorTotalComImpostos, quantidadeDisponivel, observacoes);
                Itens.Add(cotacaoItem);
            }

            return cotacaoItem;
        }

    }

   
}

using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class CotacaoItem
    {
        public virtual int Id { get; protected set; }
        public virtual Cotacao Cotacao { get; protected set; }
        public virtual ProcessoDeCotacaoItem ProcessoDeCotacaoItem { get; set; }
        public virtual bool Selecionada { get; protected set; }
        public virtual decimal ValorLiquido { get; protected set; }
        public virtual decimal ValorLiquidoInicial { get; protected set; }
        public virtual decimal ValorComImpostos { get; protected set; }
        public virtual decimal? QuantidadeAdquirida { get; protected set; }
        public virtual decimal QuantidadeDisponivel { get; protected set; }
        public virtual string Observacoes { get; protected set; }
        public virtual IList<Imposto> Impostos { get; protected set; }

        protected CotacaoItem()
        {
            Impostos = new List<Imposto>();
            Selecionada = false;
        }
        protected CotacaoItem(Cotacao cotacao, ProcessoDeCotacaoItem processoDeCotacaoItem,decimal valorTotalComImpostos, 
            decimal quantidadeDisponivel,string observacoes):this()
        {
            Cotacao = cotacao;
            ProcessoDeCotacaoItem = processoDeCotacaoItem;
            ValorComImpostos = valorTotalComImpostos;
            QuantidadeDisponivel = quantidadeDisponivel;
            Observacoes = observacoes;
            CalculaValorLiquido();
            ValorLiquidoInicial = ValorLiquido;
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

        public virtual void Atualizar(decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes)
        {
            ValorComImpostos = valorTotalComImpostos;
            QuantidadeDisponivel = quantidadeDisponivel;
            Observacoes = observacoes;
            CalculaValorLiquido();
        }

        /// <summary>
        /// remove imposto por tipo, caso exista
        /// </summary>
        /// <param name="tipoDeImposto"></param>
        private void RemoverImposto(Enumeradores.TipoDeImposto tipoDeImposto)
        {
            var imposto = Impostos.FirstOrDefault(x => x.Tipo == tipoDeImposto);
            if (imposto != null)
            {
                Impostos.Remove(imposto);
            }
        }

        private void AtualizarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal aliquota, decimal valor)
        {
            Imposto imposto = Impostos.FirstOrDefault(x => x.Tipo == tipoDeImposto);
            if (imposto != null)
            {
                imposto.Atualizar(aliquota, valor);
            }
            else
            {
                imposto = new Imposto(this, tipoDeImposto, aliquota, valor);
                Impostos.Add(imposto);
            }

        }

        public virtual void InformarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal aliquota, decimal valor)
        {
            AtualizarImposto(tipoDeImposto, aliquota, valor);

            CalculaValorLiquido();
        }


        public virtual Imposto Imposto(Enumeradores.TipoDeImposto tipo)
        {
            return Impostos.SingleOrDefault(x => x.Tipo == tipo);
        }

        public virtual void Selecionar(decimal quantidadeAdquirida)
        {
            Selecionada = true;
            QuantidadeAdquirida = quantidadeAdquirida;
        }

        public virtual void RemoverSelecao()
        {
            Selecionada = false;
            QuantidadeAdquirida = null;
        }

        #region override members

        protected bool Equals(CotacaoItem other)
        {
            return Equals(Cotacao, other.Cotacao) && Equals(ProcessoDeCotacaoItem, other.ProcessoDeCotacaoItem);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CotacaoItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Cotacao != null ? Cotacao.GetHashCode() : 0)*397) ^ (ProcessoDeCotacaoItem != null ? ProcessoDeCotacaoItem.GetHashCode() : 0);
            }
        }

        #endregion


    }

    public class CotacaoMaterialItem: CotacaoItem
    {
        public virtual decimal? Mva { get; protected set; }
        public virtual Iva Iva { get; protected set; }
        public virtual DateTime PrazoDeEntrega { get; protected set; }

        protected CotacaoMaterialItem(){}

        internal CotacaoMaterialItem(Cotacao cotacao, ProcessoDeCotacaoItem processoDeCotacaoItem, decimal? mva, DateTime prazoDeEntrega,
            decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes) 
            : base(cotacao, processoDeCotacaoItem,valorTotalComImpostos, quantidadeDisponivel, observacoes)
        {
            Mva = mva;
            PrazoDeEntrega = prazoDeEntrega;
        }

        public virtual void Atualizar(decimal valorTotalComImpostos,decimal? mva, decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            base.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);
            Mva = mva;
            PrazoDeEntrega = prazoDeEntrega;
        }
        public virtual void Selecionar(decimal quantidadeAdquirida, Iva iva)
        {
            base.Selecionar(quantidadeAdquirida);
            Iva = iva;
        }

        public virtual void RemoverSelecao(Iva iva)
        {
            base.RemoverSelecao();
            Iva = iva;
        }

        public override void InformarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal aliquota, decimal valor)
        {
            if (tipoDeImposto == Enumeradores.TipoDeImposto.IcmsSubstituicao
            && valor > 0 && (!Mva.HasValue || Mva.Value == 0))
            {
                throw new MvaNaoInformadoException();
            }

            base.InformarImposto(tipoDeImposto, aliquota, valor);
        }

    }

    public class CotacaoFreteItem: CotacaoItem
    {
        protected CotacaoFreteItem(){}
        internal CotacaoFreteItem(Cotacao cotacao, ProcessoDeCotacaoItem processoDeCotacaoItem,decimal valorTotalComImpostos, 
            decimal quantidadeDisponivel, string observacoes) : base(cotacao, processoDeCotacaoItem,valorTotalComImpostos, quantidadeDisponivel, observacoes)
        {
        }

    }
}

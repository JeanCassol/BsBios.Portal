using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class Cotacao
    {
        public virtual int Id { get; protected set; }
        //private int IdFornecedorParticipante { get; set; }
        //public virtual FornecedorParticipante FornecedorParticipante { get; protected set; }
        public virtual bool Selecionada { get; protected set; }
        public virtual decimal ValorLiquido { get; protected set; }
        public virtual decimal ValorComImpostos { get; protected set;}
        public virtual decimal? QuantidadeAdquirida { get; protected set; }
        public virtual decimal QuantidadeDisponivel { get; protected set; }
        public virtual string Observacoes { get; protected set; }
        public virtual IList<Imposto> Impostos { get; protected set; }

        protected Cotacao()
        {
            Impostos = new List<Imposto>();
            Selecionada = false;
        }


        protected Cotacao(decimal valorTotalComImpostos, decimal quantidadeDisponivel,string observacoes):this()
        {
            ValorComImpostos = valorTotalComImpostos;
            QuantidadeDisponivel = quantidadeDisponivel;
            Observacoes = observacoes;
            CalculaValorLiquido();
        }
        

        //public Cotacao(FornecedorParticipante fornecedorParticipante)
        //{
        //    //IdFornecedorParticipante = fornecedorParticipante.Id;
        //    //FornecedorParticipante = fornecedorParticipante;
            
        //}

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

        protected virtual void Selecionar(decimal quantidadeAdquirida)
        {
            Selecionada = true;
            QuantidadeAdquirida = quantidadeAdquirida;
        }

        protected virtual void RemoverSelecao()
        {
            Selecionada = false;
            QuantidadeAdquirida = null;
        }

    }

    public class CotacaoDeFrete: Cotacao
    {
        public CotacaoDeFrete(decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes)
            : base(valorTotalComImpostos, quantidadeDisponivel, observacoes){}

        protected CotacaoDeFrete(){}

        public virtual decimal? Cadencia { get; protected set; }
        public virtual string NumeroDaCondicaoGeradaNoSap { get; protected set; }


        public new virtual void Atualizar(decimal valorTotalComImpostos, decimal quantidadeDisponivel, string observacoes)
        {
            base.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);
        }


        public new virtual void Selecionar(decimal quantidadeAdquirida, decimal cadencia)
        {
            base.Selecionar(quantidadeAdquirida);
            Cadencia = cadencia;
        }

        public new virtual void RemoverSelecao()
        {
            base.RemoverSelecao();
            Cadencia = null;
        }

        protected internal virtual void InformarNumeroDaCondicao(string numeroGeradoNoSap)
        {
            NumeroDaCondicaoGeradaNoSap = numeroGeradoNoSap;
        }
        
    }

    public class CotacaoMaterial: Cotacao
    {

        public virtual CondicaoDePagamento CondicaoDePagamento { get; protected set; }
        public virtual decimal? Mva { get; protected set; }
        public virtual Iva Iva { get; protected set; }
        public virtual Incoterm Incoterm { get; protected set; }
        public virtual string DescricaoIncoterm { get; protected set; }
        public virtual DateTime PrazoDeEntrega { get; protected set; }

        protected CotacaoMaterial(){}

        public CotacaoMaterial(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoIncoterm, 
            decimal valorTotalComImpostos, decimal? mva, decimal quantidadeDisponivel,
            DateTime prazoDeEntrega, string observacoes):base(valorTotalComImpostos, quantidadeDisponivel, observacoes)
        {
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
            Mva = mva;
            PrazoDeEntrega = prazoDeEntrega;
        }

        public virtual void Atualizar(decimal valorTotalComImpostos, CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoIncoterm, 
            decimal? mva, decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            base.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);

            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
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

   
}

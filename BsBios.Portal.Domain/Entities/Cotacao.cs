using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public class Cotacao
    {
        public virtual int Id { get; protected set; }
        //private int IdFornecedorParticipante { get; set; }
        //public virtual FornecedorParticipante FornecedorParticipante { get; protected set; }
        public virtual bool Selecionada { get; protected set; }
        public virtual decimal ValorLiquido { get; protected set; }
        public virtual decimal ValorComImpostos { get; protected set;}
        public virtual decimal? QuantidadeAdquirida { get; protected set; }
        public virtual CondicaoDePagamento CondicaoDePagamento { get; protected set; }
        public virtual Iva Iva { get; protected set; }
        public virtual Incoterm Incoterm { get; protected set; }
        public virtual string DescricaoIncoterm{ get; protected set; }
        public virtual decimal? Mva { get; protected set; }
        public virtual decimal QuantidadeDisponivel { get; protected set; }
        public virtual DateTime PrazoDeEntrega { get; protected set; }
        public virtual string Observacoes { get; protected set; }
        public virtual IList<Imposto> Impostos { get; protected set; }

        protected Cotacao()
        {
            Impostos = new List<Imposto>();
            Selecionada = false;
        }
        

        public Cotacao(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoIncoterm, 
            decimal valorTotalComImpostos, decimal? mva, decimal quantidadeDisponivel,
            DateTime prazoDeEntrega, string observacoes):this()
        {
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
            ValorComImpostos = valorTotalComImpostos;
            Mva = mva;
            QuantidadeDisponivel = quantidadeDisponivel;
            PrazoDeEntrega = prazoDeEntrega;
            Observacoes = observacoes;
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

        public virtual void Atualizar(decimal valorTotalComImpostos, CondicaoDePagamento condicaoDePagamento, 
            Incoterm incoterm, string descricaoIncoterm, decimal? mva, decimal quantidadeDisponivel,
            DateTime prazoDeEntrega, string observacoes)
        {
            
            ValorComImpostos = valorTotalComImpostos;
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
            Mva = mva;
            QuantidadeDisponivel = quantidadeDisponivel;
            PrazoDeEntrega = prazoDeEntrega;
            Observacoes = observacoes;
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

        public virtual void InformarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal? aliquota, decimal? valor)
        {
            var temImposto = aliquota.HasValue && valor.HasValue;
            if (temImposto)
            {
                AtualizarImposto(tipoDeImposto, aliquota.Value, valor.Value);
            }
            else
            {
                RemoverImposto(tipoDeImposto);
            }

            CalculaValorLiquido();

            if (temImposto && tipoDeImposto == Enumeradores.TipoDeImposto.IcmsSubstituicao 
                && valor > 0 && ( !Mva.HasValue || Mva.Value == 0))
            {
                throw  new MvaNaoInformadoException();
            }


        }

        public virtual void Selecionar(decimal quantidadeAdquirida, Iva iva)
        {
            Selecionada = true;
            QuantidadeAdquirida = quantidadeAdquirida;
            Iva = iva;
        }

        public virtual Imposto Imposto(Enumeradores.TipoDeImposto tipo)
        {
            return Impostos.SingleOrDefault(x => x.Tipo == tipo);
        }

        public virtual void RemoverSelecao(Iva iva)
        {
            Selecionada = false;
            QuantidadeAdquirida = null;
            Iva = iva;
        }
    }

   
}

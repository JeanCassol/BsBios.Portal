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
        public virtual decimal ValorTotalSemImpostos { get; protected set; }
        public virtual decimal? ValorTotalComImpostos { get; protected set;}
        public virtual decimal? QuantidadeAdquirida { get; protected set; }
        public virtual CondicaoDePagamento CondicaoDePagamento { get; protected set; }
        public virtual Iva Iva { get; protected set; }
        public virtual Incoterm Incoterm { get; protected set; }
        public virtual string DescricaoIncoterm{ get; protected set; }
        public virtual decimal? Mva { get; protected set; }
        public virtual IList<Imposto> Impostos { get; protected set; }

        protected Cotacao()
        {
            Impostos = new List<Imposto>();
            Selecionada = false;
        }
        

        public Cotacao(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoIncoterm, 
            decimal valorTotalSemImpostos, decimal? valorTotalComImpostos, decimal? mva):this()
        {
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
            ValorTotalSemImpostos = valorTotalSemImpostos;
            ValorTotalComImpostos = valorTotalComImpostos;
            Mva = mva;
        }
        

        //public Cotacao(FornecedorParticipante fornecedorParticipante)
        //{
        //    //IdFornecedorParticipante = fornecedorParticipante.Id;
        //    //FornecedorParticipante = fornecedorParticipante;
            
        //}

        public virtual void Atualizar(decimal valorTotalSemImpostos, decimal? valorTotalComImpostos, CondicaoDePagamento condicaoDePagamento, 
            Incoterm incoterm, string descricaoIncoterm, decimal? mva)
        {
            ValorTotalSemImpostos = valorTotalSemImpostos;
            ValorTotalComImpostos = valorTotalComImpostos;
            CondicaoDePagamento = condicaoDePagamento;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
            Mva = mva;
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

            if (Impostos.Any() &&  (!ValorTotalComImpostos.HasValue || ValorTotalComImpostos.Value == 0) )
            {
                throw new ValorTotalComImpostosObrigatorioException();
            }

            if (temImposto && tipoDeImposto == Enumeradores.TipoDeImposto.IcmsSubstituicao && (!Mva.HasValue || Mva.Value == 0))
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
    }

   
}

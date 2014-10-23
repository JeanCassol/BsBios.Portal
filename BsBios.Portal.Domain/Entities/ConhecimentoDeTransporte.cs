using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class ConhecimentoDeTransporte : IAggregateRoot
    {

        private readonly IList<NotaFiscalDeConhecimentoDeTransporte> _notasFiscais;
        private IList<OrdemDeTransporte> OrdensVinculadas { get; set; }

        protected ConhecimentoDeTransporte() { }

        public ConhecimentoDeTransporte(string chaveEletronica, string cnpjDoFornecedor,  string cnpjDaTransportadora,
            DateTime dataDeEmissao, string serie, string numero, string numeroDoContrato, decimal valorRealDoFrete, decimal pesoTotalDaCarga)
        {
            this.ChaveEletronica = chaveEletronica;
            this.CnpjDoFornecedor = cnpjDoFornecedor;
            this.CnpjDaTransportadora = cnpjDaTransportadora;
            this.DataDeEmissao = dataDeEmissao;
            this.Serie = serie;
            this.Numero = numero;
            this.NumeroDoContrato = numeroDoContrato;
            this.ValorRealDoFrete = valorRealDoFrete;
            this.PesoTotalDaCarga = pesoTotalDaCarga;
            this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.NaoAtribuido;

            _notasFiscais = new List<NotaFiscalDeConhecimentoDeTransporte>();
            this.OrdensVinculadas = new List<OrdemDeTransporte>();

        }

        public static class Expressions
        {
            public static Expression<Func<ConhecimentoDeTransporte, IEnumerable<NotaFiscalDeConhecimentoDeTransporte>>> NotasFiscais = x => x._notasFiscais;
            public static Expression<Func<ConhecimentoDeTransporte, IEnumerable<OrdemDeTransporte>>> OrdensDeTransporteVinculadas = x => x.OrdensVinculadas;
        }

        public virtual string ChaveEletronica { get; protected internal set; }
        public virtual string CnpjDoFornecedor { get; protected internal set; }
        public virtual Fornecedor Fornecedor { get; protected internal set; }
        public virtual string CnpjDaTransportadora { get; protected internal set; }
        public virtual Fornecedor Transportadora { get; protected internal set; }
        public virtual DateTime DataDeEmissao { get; protected internal set; }
        public virtual string Numero { get; protected internal set; }
        public virtual string Serie { get; protected internal set; }
        public virtual string NumeroDoContrato { get; protected internal set; }
        public virtual decimal ValorRealDoFrete { get; protected internal set; }
        public virtual decimal PesoTotalDaCarga { get; protected internal set; }
        public virtual Enumeradores.StatusDoConhecimentoDeTransporte Status { get; protected internal set; }
        public virtual string MensagemDeErroDeProcessamento { get; protected internal set; }
        


        public virtual ReadOnlyCollection<NotaFiscalDeConhecimentoDeTransporte> NotasFiscais
        {
            get
            {
                return new ReadOnlyCollection<NotaFiscalDeConhecimentoDeTransporte>(this._notasFiscais);
            }
        }

        public virtual ReadOnlyCollection<OrdemDeTransporte> OrdensDeTransporteCandidatas
        {
            get
            {
                return new ReadOnlyCollection<OrdemDeTransporte>(this.OrdensVinculadas);
            }
        }


        public virtual void AdicionarNotaFiscal(string chave, string numero, string serie)
        {
            var notaFiscalDeConhecimentoDeTransporte = new NotaFiscalDeConhecimentoDeTransporte(chave, numero, serie);
            _notasFiscais.Add(notaFiscalDeConhecimentoDeTransporte);
            
        }

        public virtual void VincularComOrdem(OrdemDeTransporte ordemDeTransporte)
        {
            try
            {
                this.OrdensVinculadas.Add(ordemDeTransporte);

                var coleta = new Coleta(null, null,this.DataDeEmissao, this.DataDeEmissao);

                int quantidadeDeNotas = this.NotasFiscais.Count;
                decimal pesoRateado =  Math.Round(this.PesoTotalDaCarga / quantidadeDeNotas,3);
                decimal valorRateado = Math.Round(this.ValorRealDoFrete / quantidadeDeNotas,2);

                foreach (NotaFiscalDeConhecimentoDeTransporte notaDeConhecimento in this.NotasFiscais)
                {
                    var notaFiscalDeColeta = new NotaFiscalDeColeta(notaDeConhecimento.Numero, notaDeConhecimento.Serie, 
                        this.Numero, this.DataDeEmissao,pesoRateado, valorRateado);

                    coleta.AdicionarNotaFiscal(notaFiscalDeColeta, ordemDeTransporte.PrecoUnitario);

                }

                ordemDeTransporte.AdicionarColeta(coleta);

                this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.Atribuido;

            }
            catch (Exception exception)
            {
                this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.Erro;
                this.MensagemDeErroDeProcessamento = exception.Message;
            }
        }

        public virtual void IndicarOrdensCandidatas(IList<OrdemDeTransporte> ordensCandidatas)
        {
            this.OrdensVinculadas = ordensCandidatas;
        }


        protected internal virtual void AtribuirFornecedorDaMercadoria(Fornecedor fornecedor)
        {
            if (fornecedor == null)
            {
                this.MensagemDeErroDeProcessamento = "Fornecedor da Mercadoria não encontrado";
            }
            else
            {
                this.Fornecedor = fornecedor;
            }
        }

        protected internal virtual void AtribuirTransportadora(Fornecedor transportadora)
        {
            if (transportadora == null)
            {
                this.MensagemDeErroDeProcessamento = "Transportadora não encontrada";
            }
            else
            {
                this.Transportadora = transportadora;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class ConhecimentoDeTransporte : IAggregateRoot
    {

        private readonly IList<NotaFiscalDeConhecimentoDeTransporte> _notasFiscais;
        // ReSharper disable once InconsistentNaming
        private IList<OrdemDeTransporte> _ordensDeTransporte;

        protected ConhecimentoDeTransporte() { }

        public ConhecimentoDeTransporte(string chaveEletronica, string cnpjDoFornecedor,  string cnpjDaTransportadora,
            DateTime dataDeEmissao, string serie, string numero, string numeroDoContrato, decimal valorRealDoFrete, decimal pesoTotalDaCargaEmQuilos)
        {
            this.ChaveEletronica = chaveEletronica;
            this.CnpjDoFornecedor = cnpjDoFornecedor;
            this.CnpjDaTransportadora = cnpjDaTransportadora;
            this.DataDeEmissao = dataDeEmissao;
            this.Serie = serie;
            this.Numero = numero;
            this.NumeroDoContrato = numeroDoContrato;
            this.ValorRealDoFrete = valorRealDoFrete;
            this.PesoTotalDaCargaEmToneladas = pesoTotalDaCargaEmQuilos / 1000;
            this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.SemOrdemDeTransporte;

            this._notasFiscais = new List<NotaFiscalDeConhecimentoDeTransporte>();
            this._ordensDeTransporte = new List<OrdemDeTransporte>();

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
        public virtual decimal PesoTotalDaCargaEmToneladas { get; protected internal set; }
        public virtual Enumeradores.StatusDoConhecimentoDeTransporte Status { get; protected internal set; }
        public virtual string MensagemDeErroDeProcessamento { get; protected internal set; }


        public virtual IEnumerable<NotaFiscalDeConhecimentoDeTransporte> NotasFiscais
        {
            get
            {
                return this._notasFiscais;
            }
        }

        public virtual IEnumerable<OrdemDeTransporte> OrdensDeTransporte
        {
            get
            {
                return this._ordensDeTransporte;
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

                ordemDeTransporte.InformarConhecimentoDeTransporte(this);

                this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.Atribuido;

                this._ordensDeTransporte.Clear();
                this._ordensDeTransporte.Add(ordemDeTransporte);

            }
            catch (Exception exception)
            {
                this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.Erro;
                this.MensagemDeErroDeProcessamento = exception.Message;
            }
        }

        /// <summary>
        /// quando tem mais de uma ordem candidata coloca para status "Não atribuído"
        /// </summary>
        /// <param name="ordensCandidatas"></param>
        public virtual void IndicarOrdensCandidatas(IList<OrdemDeTransporte> ordensCandidatas)
        {
            this._ordensDeTransporte = ordensCandidatas;
            this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.NaoAtribuido;
        }


        protected internal virtual void AtribuirFornecedorDaMercadoria(Fornecedor fornecedor)
        {
            if (fornecedor == null)
            {
                this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.Erro;
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
                this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.Erro;
                this.MensagemDeErroDeProcessamento = "Transportadora não encontrada";
            }
            else
            {
                this.Transportadora = transportadora;
            }
        }

        public virtual void InformarErroDeProcessamento(string mensagemDeErro)
        {
            this.MensagemDeErroDeProcessamento = mensagemDeErro;
            this.Status = Enumeradores.StatusDoConhecimentoDeTransporte.Erro;
        }
    }
}
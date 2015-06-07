using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Entities
{
    public class OrdemDeTransporte : IAggregateRoot
    {

        internal OrdemDeTransporte(ProcessoDeCotacaoDeFrete processoDeCotacao, Fornecedor fornecedor, 
            decimal quantidadeAdquirida, decimal precoUnitario, decimal cadencia):this()
        {
            ProcessoDeCotacaoDeFrete = processoDeCotacao;
            Fornecedor = fornecedor;
            QuantidadeAdquirida = quantidadeAdquirida;
            PrecoUnitario = precoUnitario;
            QuantidadeLiberada = quantidadeAdquirida;
            Cadencia = cadencia;
            StatusParaColeta = Enumeradores.StatusParaColeta.Aberto;
        }

        protected OrdemDeTransporte()
        {
            Coletas = new List<Coleta>();
        }

        public virtual int Id { get; protected set; }
        public virtual ProcessoDeCotacaoDeFrete ProcessoDeCotacaoDeFrete { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual decimal QuantidadeAdquirida { get; protected set; }
        public virtual decimal QuantidadeLiberada { get; protected set; }
        public virtual decimal QuantidadeDeTolerancia { get; protected set; }

        public virtual decimal QuantidadeColetada { get; protected set; }

        public virtual  decimal QuantidadeRealizada { get; protected set; }

        public virtual decimal PrecoUnitario { get; protected set; }
        public virtual decimal Cadencia { get; set; }

        public virtual decimal ValorPlanejado{ get; protected set; }
        public virtual decimal ValorReal { get; protected set; }

        public virtual IList<Coleta> Coletas { get; protected set; }
        public virtual Enumeradores.StatusParaColeta StatusParaColeta { get; protected set; }
        public virtual string ObservacaoDeFechamento { get; protected set; }
        public virtual Enumeradores.MotivoDeFechamentoDaOrdemDeTransporte MotivoDeFechamento { get; protected set; }

        public virtual void AlterarQuantidades(decimal novaQuantidadeLiberada, decimal novaQuantidadeDeTolerancia)
        {
            if (this.StatusParaColeta == Enumeradores.StatusParaColeta.Fechado)
            {
                throw new AlterarOrdemDeTransporteFechadaException();
            }
            if ((novaQuantidadeLiberada + novaQuantidadeDeTolerancia) < QuantidadeColetada)
            {
                throw new QuantidadeLiberadaAbaixoDaQuantidadeColetadaException(novaQuantidadeLiberada, QuantidadeColetada);
            }
            QuantidadeLiberada = novaQuantidadeLiberada;
            QuantidadeDeTolerancia = novaQuantidadeDeTolerancia;
        }

        private void AtualizarQuantidadeColetada(bool validarNovaQuantidade)
        {
            this.QuantidadeColetada = Coletas.Sum(x => x.Peso);
            decimal quantidadeMaximaPermitida = QuantidadeLiberada + QuantidadeDeTolerancia;
            if (validarNovaQuantidade && QuantidadeColetada > quantidadeMaximaPermitida)
            {
                throw new QuantidadeColetadaUltrapassouQuantidadeLiberadaException(QuantidadeColetada, quantidadeMaximaPermitida);
            }
            
        }

        public virtual void AdicionarColeta(Coleta coleta)
        {
            if (this.StatusParaColeta == Enumeradores.StatusParaColeta.Fechado)
            {
                throw new AlterarOrdemDeTransporteFechadaException();
            }

            Coletas.Add(coleta);
            
            try
            {
                AtualizarQuantidadeColetada(true);
            }
            catch (QuantidadeColetadaUltrapassouQuantidadeLiberadaException)
            {
                this.Coletas.Remove(coleta);
                throw;
            }
            
            CalcularValoresTotais();

        }

        public virtual void AtualizarColeta(ColetaSalvarVm coletaSalvarVm)
        {
            if (this.StatusParaColeta == Enumeradores.StatusParaColeta.Fechado)
            {
                throw new AlterarOrdemDeTransporteFechadaException();
            }
            var coleta = Coletas.Single(x => x.Id == coletaSalvarVm.IdColeta);

            if (coleta.Realizado)
            {
                throw new Exception("Não é possível alterar uma Coleta que já foi realizada.");
            }

            coleta.Atualizar(coletaSalvarVm,PrecoUnitario);

            AtualizarQuantidadeColetada(true);

        }

        public virtual void RemoverColeta(int idDaColeta)
        {
            if (this.StatusParaColeta == Enumeradores.StatusParaColeta.Fechado)
            {
                throw new AlterarOrdemDeTransporteFechadaException();
            }
            var coleta = Coletas.Single(x => x.Id == idDaColeta);
            if (coleta.Realizado)
            {
                throw new Exception("Não é possível excluir uma Coleta que já foi realizada.");
            }

            Coletas.Remove(coleta);

            AtualizarQuantidadeColetada(false);
        }

        private void RealizarColeta(Coleta coleta)
        {
            coleta.Realizar();
            QuantidadeRealizada = Coletas.Where(x => x.Realizado).Sum(x => x.Peso);
            
        }

        public virtual void RealizarColeta(int idDaColeta)
        {
            Coleta coleta = Coletas.Single(c => c.Id == idDaColeta);
            RealizarColeta(coleta);
        }

        public virtual Coleta RealizarColeta(string numero, string serie)
        {
            Coleta coleta = this.Coletas.Single(c => c.NotasFiscais.Any(nf => nf.Numero == numero && nf.Serie == serie));
            RealizarColeta(coleta);
            return coleta;
        }


        public virtual void FecharParaColeta(Enumeradores.MotivoDeFechamentoDaOrdemDeTransporte motivo, string observacao)
        {
            if (this.StatusParaColeta  == Enumeradores.StatusParaColeta.Fechado)
            {
                throw new Exception("A Ordem de Transporte já está fechada.");
                
            }
            this.StatusParaColeta = Enumeradores.StatusParaColeta.Fechado;
            this.QuantidadeLiberada = this.QuantidadeColetada;
            this.MotivoDeFechamento = motivo;
            this.ObservacaoDeFechamento = observacao;
        }

        private void CalcularValoresTotais()
        {
            this.ValorPlanejado = this.QuantidadeColetada * PrecoUnitario;
            this.ValorReal = this.Coletas.Sum(x => x.ValorDoFrete);
        }

    }
}
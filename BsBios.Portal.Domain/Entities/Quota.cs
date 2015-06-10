using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Entities
{
    public class Quota: IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual MaterialDeCarga Material { get; protected set; }
        public virtual Enumeradores.FluxoDeCarga FluxoDeCarga { get; protected set; }
        public virtual Terminal Terminal { get; protected set; }
        public virtual DateTime Data { get; protected set; }
        /// <summary>
        /// total de peso liberado para o fornecedor fazer carregamento ou descarregamento
        /// </summary>
        public virtual decimal PesoTotal { get; protected set; }
        /// <summary>
        /// total de peso agendado para fazer carregamento ou descarregamento
        /// </summary>
        public virtual decimal PesoAgendado { get; protected set; }
        /// <summary>
        /// somatório do peso das cargas que foram agendadadas e já foram carregadas ou descarregadas (conforme o fluxo)
        /// </summary>
        public virtual decimal PesoRealizado { get; protected set; }
        public virtual IList<AgendamentoDeCarga> Agendamentos { get; protected set; }

        /// <summary>
        /// Peso que ainda não foi agendado
        /// </summary>
        public virtual decimal PesoDisponivel
        {
            get { return PesoTotal - PesoAgendado ; }
        }

        protected Quota()
        {
            Agendamentos = new List<AgendamentoDeCarga>();
        }

        public Quota(MaterialDeCarga materialDeCarga, Enumeradores.FluxoDeCarga fluxoDeCarga, Fornecedor fornecedor, Terminal terminal, DateTime data, decimal pesoTotal):this()
        {
            Fornecedor = fornecedor;
            Terminal = terminal;
            Data = data;
            PesoTotal = pesoTotal;
            Material = materialDeCarga;
            FluxoDeCarga = fluxoDeCarga;
        }

        private void ValidaPeso()
        {
            if (PesoAgendado > PesoTotal)
            {
                throw new PesoAgendadoSuperiorAoPesoDaQuotaException(PesoAgendado, PesoTotal);
            }
            
        }

        public virtual void AlterarPeso(decimal peso)
        {
            PesoTotal = peso;
            ValidaPeso();
        }

        private void CalculaPesoAgendado()
        {
            PesoAgendado = Agendamentos.Sum(x => x.PesoTotal);
            ValidaPeso();
        }

        private void CalculaPesoRealizado()
        {
            PesoRealizado = Agendamentos.Where(x => x.Realizado).Sum(y => y.PesoTotal);
        }


        #region Equality Members

        protected bool Equals(Quota other)
        {
            return FluxoDeCarga == other.FluxoDeCarga && Equals(Fornecedor, other.Fornecedor) && string.Equals(Terminal, other.Terminal) && Data.Equals(other.Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Quota) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) FluxoDeCarga;
                hashCode = (hashCode*397) ^ (Fornecedor != null ? Fornecedor.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Terminal != null ? Terminal.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Data.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        private void VerificaSeExisteApenasUmAgendamentoParaCadaPlaca()
        {
            var agendamentosDuplicados =
                (from agendamento in Agendamentos
                 where !agendamento.Realizado
                 group agendamento by agendamento.Placa.ToUpper()
                 into grouped
                 where grouped.Count() > 1
                 select grouped.Key
                ).ToList();

            if (agendamentosDuplicados.Count > 0)
            {
                throw new AgendamentosSimultaneosParaMesmaPlacaException(agendamentosDuplicados);
            }
        }

        public virtual AgendamentoDeCarregamento InformarAgendamento(AgendamentoDeCarregamentoCadastroVm agendamentoDeCarregamentoCadastroVm)
        {
            AgendamentoDeCarregamento agendamento;
            if (agendamentoDeCarregamentoCadastroVm.IdAgendamento == 0)
            {
                var factory = new AgendamentoDeCarregamentoFactory(agendamentoDeCarregamentoCadastroVm.Motorista, agendamentoDeCarregamentoCadastroVm.Destino, agendamentoDeCarregamentoCadastroVm.Peso);
                agendamento = (AgendamentoDeCarregamento)factory.Construir(this, agendamentoDeCarregamentoCadastroVm.Placa);
                Agendamentos.Add(agendamento);
            }
            else
            {
                agendamento = (AgendamentoDeCarregamento) Agendamentos.Single(x => x.Id == agendamentoDeCarregamentoCadastroVm.IdAgendamento);
                agendamento.Atualizar(agendamentoDeCarregamentoCadastroVm);
            }
            VerificaSeExisteApenasUmAgendamentoParaCadaPlaca();
            CalculaPesoAgendado();

            return agendamento;
        }

        public virtual void RemoverAgendamento(int idAgendamento)
        {
            if (DateTime.Today >= Data)
            {
                throw new DataDeAgendamentoExpiradaException("Não é permitido remover um agendamento com data igual ou anterior ou à data atual.");
            }
            
            var agendamento = Agendamentos.Single(x => x.Id == idAgendamento);
            if (agendamento.Realizado)
            {
                throw new OperacaoNaoPermitidaParaAgendamentoRealizadoException("Não é permitido remover um agendamento que já foi realizado.");
            }
            Agendamentos.Remove(agendamento);
            CalculaPesoAgendado();
        }

        public virtual AgendamentoDeDescarregamento InformarAgendamento(AgendamentoDeDescarregamentoSalvarVm agendamentoDeDescarregamentoSalvarVm)
        {
            AgendamentoDeDescarregamento agendamento;
            if (agendamentoDeDescarregamentoSalvarVm.IdAgendamento == 0)
            {
                var factory = new AgendamentoDeDescarregamentoFactory();
                foreach (var notaFiscal in agendamentoDeDescarregamentoSalvarVm.NotasFiscais)
                {
                    factory.AdicionarNotaFiscal(notaFiscal);
                    
                }
                agendamento = (AgendamentoDeDescarregamento)factory.Construir(this, agendamentoDeDescarregamentoSalvarVm.Placa);
                Agendamentos.Add(agendamento);
            }
            else
            {
                agendamento = (AgendamentoDeDescarregamento)Agendamentos.Single(x => x.Id == agendamentoDeDescarregamentoSalvarVm.IdAgendamento);

                agendamento.Atualizar(agendamentoDeDescarregamentoSalvarVm);
            }
            VerificaSeExisteApenasUmAgendamentoParaCadaPlaca();
            CalculaPesoAgendado();
            return agendamento;
        }

        public virtual void RealizarAgendamento(int idAgendamento)
        {
            AgendamentoDeCarga agendamento = Agendamentos.Single(x => x.Id == idAgendamento);
            agendamento.Realizar();
            CalculaPesoRealizado();
        }
    }
}
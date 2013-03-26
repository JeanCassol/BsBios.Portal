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
        public virtual Enumeradores.MaterialDeCarga Material { get; protected set; }
        public virtual Enumeradores.FluxoDeCarga FluxoDeCarga { get; protected set; }
        public virtual string CodigoTerminal { get; protected set; }
        public virtual DateTime Data { get; protected set; }
        public virtual decimal PesoTotal { get; protected set; }
        public virtual decimal PesoAgendado { get; protected set; }
        public virtual IList<AgendamentoDeCarga> Agendamentos { get; protected set; }

        public virtual decimal PesoDisponivel
        {
            get { return PesoTotal - PesoAgendado ; }
        }

        protected Quota()
        {
            Agendamentos = new List<AgendamentoDeCarga>();
        }

        public Quota(Enumeradores.MaterialDeCarga materialDeCarga, Fornecedor fornecedor, string terminal, DateTime data, decimal pesoTotal):this()
        {
            Fornecedor = fornecedor;
            CodigoTerminal = terminal;
            Data = data;
            PesoTotal = pesoTotal;
            Material = materialDeCarga;
            if (materialDeCarga == Enumeradores.MaterialDeCarga.Soja)
            {
                FluxoDeCarga = Enumeradores.FluxoDeCarga.Descarregamento;
            }
            if (materialDeCarga == Enumeradores.MaterialDeCarga.Farelo)
            {
                FluxoDeCarga = Enumeradores.FluxoDeCarga.Carregamento;
            }
        }

        public virtual void AlterarPeso(decimal peso)
        {
            PesoTotal = peso;
        }



        private void CalculaPesoAgendado()
        {
            PesoAgendado = Agendamentos.Sum(x => x.PesoTotal);
            if (PesoAgendado > PesoTotal)
            {
                throw new PesoAgendadoSuperiorAoPesoDaQuotaException(PesoAgendado, PesoTotal);
            }
        }


        #region Equality Members

        protected bool Equals(Quota other)
        {
            return FluxoDeCarga == other.FluxoDeCarga && Equals(Fornecedor, other.Fornecedor) && string.Equals(CodigoTerminal, other.CodigoTerminal) && Data.Equals(other.Data);
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
                hashCode = (hashCode*397) ^ (CodigoTerminal != null ? CodigoTerminal.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Data.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        public virtual void InformarAgendamento(AgendamentoDeCarregamentoCadastroVm agendamentoDeCarregamentoCadastroVm)
        {
            if (agendamentoDeCarregamentoCadastroVm.IdAgendamento == 0)
            {
                var factory = new AgendamentoDeCarregamentoFactory(agendamentoDeCarregamentoCadastroVm.Peso);
                var agendamentoDeCarregamento = (AgendamentoDeCarregamento)factory.Construir(this, agendamentoDeCarregamentoCadastroVm.Placa);
                Agendamentos.Add(agendamentoDeCarregamento);
            }
            else
            {
                var agendamento = (AgendamentoDeCarregamento) Agendamentos.Single(x => x.Id == agendamentoDeCarregamentoCadastroVm.IdAgendamento);
                agendamento.Atualizar(agendamentoDeCarregamentoCadastroVm);
            }
            CalculaPesoAgendado();
        }

        public virtual void RemoverAgendamento(int idAgendamento)
        {
            var agendamento = Agendamentos.Single(x => x.Id == idAgendamento);
            Agendamentos.Remove(agendamento);
            CalculaPesoAgendado();
        }

        public virtual void InformarAgendamento(AgendamentoDeDescarregamentoSalvarVm agendamentoDeDescarregamentoSalvarVm)
        {
            if (agendamentoDeDescarregamentoSalvarVm.IdAgendamento == 0)
            {
                var factory = new AgendamentoDeDescarregamentoFactory();
                foreach (var notaFiscal in agendamentoDeDescarregamentoSalvarVm.NotasFiscais)
                {
                    factory.AdicionarNotaFiscal(notaFiscal);
                    
                }
                var agendamentoDeCarregamento = (AgendamentoDeDescarregamento)factory.Construir(this, agendamentoDeDescarregamentoSalvarVm.Placa);
                Agendamentos.Add(agendamentoDeCarregamento);
            }
            else
            {
                var agendamento = (AgendamentoDeDescarregamento)Agendamentos.Single(x => x.Id == agendamentoDeDescarregamentoSalvarVm.IdAgendamento);
                agendamento.Atualizar(agendamentoDeDescarregamentoSalvarVm);
            }
            CalculaPesoAgendado();
        }
    }
}
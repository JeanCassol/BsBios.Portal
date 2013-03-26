using System;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AgendamentoDeCargaService : IAgendamentoDeCargaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotas _quotas;
        private readonly IBuilder<Quota, QuotaPesoVm> _builderQuotaPeso;

        public AgendamentoDeCargaService(IUnitOfWork unitOfWork, IQuotas quotas, IBuilder<Quota, QuotaPesoVm> builderQuotaPeso)
        {
            _unitOfWork = unitOfWork;
            _quotas = quotas;
            _builderQuotaPeso = builderQuotaPeso;
        }

        public QuotaPesoVm SalvarAgendamentoDeCarregamento(AgendamentoDeCarregamentoCadastroVm agendamentoDeCarregamentoCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                Quota quota = _quotas.BuscaPorId(agendamentoDeCarregamentoCadastroVm.IdQuota);
                quota.InformarAgendamento(agendamentoDeCarregamentoCadastroVm);
                
                _quotas.Save(quota);
                _unitOfWork.Commit();
                return _builderQuotaPeso.BuildSingle(quota);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public QuotaPesoVm SalvarAgendamentoDeDescarregamento(AgendamentoDeDescarregamentoSalvarVm agendamentoDeDescarregamentoSalvarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Quota quota = _quotas.BuscaPorId(agendamentoDeDescarregamentoSalvarVm.IdQuota);
                quota.InformarAgendamento(agendamentoDeDescarregamentoSalvarVm);
                _quotas.Save(quota);
                
                _unitOfWork.Commit();

                return _builderQuotaPeso.BuildSingle(quota);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public QuotaPesoVm ExcluirAgendamento(int idQuota, int idAgendamento)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                Quota quota = _quotas.BuscaPorId(idQuota);
                quota.RemoverAgendamento(idAgendamento);
                _unitOfWork.Commit();
                return _builderQuotaPeso.BuildSingle(quota);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
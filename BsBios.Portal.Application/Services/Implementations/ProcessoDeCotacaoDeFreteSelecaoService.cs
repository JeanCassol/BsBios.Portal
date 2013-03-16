using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoDeFreteSelecaoService : IProcessoDeCotacaoDeFreteSelecaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public ProcessoDeCotacaoDeFreteSelecaoService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
        }

        public void AtualizarSelecao(ProcessoDeCotacaoDeFreteSelecaoAtualizarVm processoDeCotacaoSelecaoAtualizarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeFrete) _processosDeCotacao.BuscaPorId(processoDeCotacaoSelecaoAtualizarVm.IdProcessoCotacao)
                    .Single().CastEntity();
                foreach (var cotacaoSelecaoVm in processoDeCotacaoSelecaoAtualizarVm.Cotacoes)
                {
                    if (cotacaoSelecaoVm.Selecionada)
                    {
                        processoDeCotacao.SelecionarCotacao(cotacaoSelecaoVm.IdCotacao, cotacaoSelecaoVm.QuantidadeAdquirida.Value);
                    }
                    else
                    {
                        processoDeCotacao.RemoverSelecaoDaCotacao(cotacaoSelecaoVm.IdCotacao);
                    }

                }

                _processosDeCotacao.Save(processoDeCotacao);

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}

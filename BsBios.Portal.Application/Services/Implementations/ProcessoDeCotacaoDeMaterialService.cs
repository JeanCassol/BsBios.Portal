using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoDeMaterialService : IProcessoDeCotacaoDeMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public ProcessoDeCotacaoDeMaterialService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
        }


        public void AtualizarProcesso(ProcessoDeCotacaoAtualizarVm atualizacaoDoProcessoDeCotacaoVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeMaterial) _processosDeCotacao.BuscaPorId(atualizacaoDoProcessoDeCotacaoVm.Id).Single();
                processoDeCotacao.Atualizar(atualizacaoDoProcessoDeCotacaoVm.DataLimiteRetorno, atualizacaoDoProcessoDeCotacaoVm.Requisitos);
                _processosDeCotacao.Save(processoDeCotacao);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public VerificacaoDeQuantidadeAdquiridaVm VerificarQuantidadeAdquirida(int idProcessoCotacao, decimal quantidadeTotalAdquirida)
        {
            var processoDeCotacao =  _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
            return new VerificacaoDeQuantidadeAdquiridaVm
                {
                    QuantidadeSolicitadaNoProcessoDeCotacao = processoDeCotacao.Quantidade,
                    SuperouQuantidadeSolicitada = processoDeCotacao.SuperouQuantidadeSolicitada(quantidadeTotalAdquirida)
                };
        }
    }
}
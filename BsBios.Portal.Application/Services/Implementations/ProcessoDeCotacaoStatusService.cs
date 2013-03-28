using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoStatusService : IProcessoDeCotacaoStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IGeradorDeEmail _geradorDeEmail;
        public  IComunicacaoSap ComunicacaoSap { get; set; }

        public ProcessoDeCotacaoStatusService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IGeradorDeEmail geradorDeEmail)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _geradorDeEmail = geradorDeEmail;
        }


        public void AbrirProcesso(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.Abrir();
                _geradorDeEmail.AberturaDoProcessoDeCotacaoDeFrete(processoDeCotacao);
                _processosDeCotacao.Save(processoDeCotacao);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void FecharProcesso(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.Fechar();
                ComunicacaoSap.EfetuarComunicacao(processoDeCotacao);
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
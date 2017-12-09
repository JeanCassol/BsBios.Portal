using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AtualizadorDeCotacaoDeFrete : IAtualizadorDeCotacaoDeFrete
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public AtualizadorDeCotacaoDeFrete(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao)
        {
            _processosDeCotacao = processosDeCotacao;
            _unitOfWork = unitOfWork;
        }

        public void Atualizar(CotacaoInformarVm cotacaoInformarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeFrete) _processosDeCotacao.BuscaPorId(cotacaoInformarVm.IdProcessoCotacao).Single();
                ProcessoDeCotacaoDeFreteItem item = processoDeCotacao.ObterItem();

                processoDeCotacao.InformarCotacao(cotacaoInformarVm.CodigoFornecedor,cotacaoInformarVm.ValorComImpostos ?? (item.ValorFechado ?? 0),
                    cotacaoInformarVm.QuantidadeDisponivel.Value, cotacaoInformarVm.ObservacoesDoFornecedor);

                _processosDeCotacao.Save(processoDeCotacao);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
            
        }

        public void SairDoProcesso(int idDoProcessoDeCotacao, string codigoDoFornecedor)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idDoProcessoDeCotacao).Single();

                processoDeCotacao.DesativarParticipante(codigoDoFornecedor);

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
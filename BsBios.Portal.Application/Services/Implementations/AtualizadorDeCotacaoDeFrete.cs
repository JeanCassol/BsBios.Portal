using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
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
                var processoDeCotacao = (ProcessoDeCotacaoDeFrete)   _processosDeCotacao.BuscaPorId(cotacaoInformarVm.IdProcessoCotacao).Single();

                ProcessoDeCotacaoItem item = processoDeCotacao.Itens.First();

                processoDeCotacao.InformarCotacao(cotacaoInformarVm.CodigoFornecedor,item, cotacaoInformarVm.ValorComImpostos.Value,
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
    }
}
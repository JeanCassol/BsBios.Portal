using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AtualizadorDeCotacao : IAtualizadorDeCotacao
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IIvas _ivas;
        private readonly IIncoterms _incoterms;
        private readonly ICondicoesDePagamento _condicoesDePagamento;

        public AtualizadorDeCotacao(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IIvas ivas, 
            IIncoterms incoterms, ICondicoesDePagamento condicoesDePagamento)
        {
            _processosDeCotacao = processosDeCotacao;
            _ivas = ivas;
            _incoterms = incoterms;
            _condicoesDePagamento = condicoesDePagamento;
            _unitOfWork = unitOfWork;
        }

        public void Atualizar(CotacaoInformarVm cotacaoAtualizarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(cotacaoAtualizarVm.IdProcessoCotacao).Single();
                //descomentar depois de ver os parâmetros
                //processoDeCotacao.AtualizarCotacao();
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
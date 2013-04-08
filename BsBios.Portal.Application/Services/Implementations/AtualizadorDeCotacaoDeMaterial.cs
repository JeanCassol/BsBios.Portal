using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AtualizadorDeCotacaoDeMaterial : AtualizadorDeImpostosDaCotacao, IAtualizadorDeCotacaoDeMaterial
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IIncoterms _incoterms;
        private readonly ICondicoesDePagamento _condicoesDePagamento;

        public AtualizadorDeCotacaoDeMaterial(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao,  
            IIncoterms incoterms, ICondicoesDePagamento condicoesDePagamento)
        {
            _processosDeCotacao = processosDeCotacao;
            _incoterms = incoterms;
            _condicoesDePagamento = condicoesDePagamento;
            _unitOfWork = unitOfWork;
        }

        public void Atualizar(CotacaoMaterialInformarVm cotacaoInformarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeMaterial)   _processosDeCotacao.BuscaPorId(cotacaoInformarVm.IdProcessoCotacao).Single();
                CondicaoDePagamento condicaoDePagamento = _condicoesDePagamento.BuscaPeloCodigo(cotacaoInformarVm.CodigoCondicaoPagamento);
                Incoterm incoterm = _incoterms.BuscaPeloCodigo(cotacaoInformarVm.CodigoIncoterm).Single();

                var cotacao = processoDeCotacao.InformarCotacao(cotacaoInformarVm.CodigoFornecedor,condicaoDePagamento, incoterm, 
                    cotacaoInformarVm.DescricaoIncoterm, cotacaoInformarVm.ValorComImpostos.Value,
                    cotacaoInformarVm.Mva, cotacaoInformarVm.QuantidadeDisponivel.Value, Convert.ToDateTime(cotacaoInformarVm.PrazoDeEntrega), 
                    cotacaoInformarVm.ObservacoesDoFornecedor);
                AtualizarImpostos(cotacao, cotacaoInformarVm.Impostos);

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
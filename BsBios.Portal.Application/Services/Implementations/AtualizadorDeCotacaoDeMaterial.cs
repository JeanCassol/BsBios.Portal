using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
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

        public int AtualizarCotacao(CotacaoMaterialInformarVm cotacaoInformarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeMaterial)   _processosDeCotacao.BuscaPorId(cotacaoInformarVm.IdProcessoCotacao).Single();
                CondicaoDePagamento condicaoDePagamento = _condicoesDePagamento.BuscaPeloCodigo(cotacaoInformarVm.CodigoCondicaoPagamento);
                Incoterm incoterm = _incoterms.BuscaPeloCodigo(cotacaoInformarVm.CodigoIncoterm).Single();

                var cotacao = processoDeCotacao.InformarCotacao(cotacaoInformarVm.CodigoFornecedor,condicaoDePagamento, incoterm, 
                    cotacaoInformarVm.DescricaoIncoterm);

                _processosDeCotacao.Save(processoDeCotacao);
                _unitOfWork.Commit();
                return cotacao.Id;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
            
        }

        public void AtualizarItemDaCotacao(CotacaoMaterialItemInformarVm cotacaoMaterialItemInformarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeMaterial)_processosDeCotacao.BuscaPorId(cotacaoMaterialItemInformarVm.IdProcessoCotacao).Single();
                CotacaoItem cotacaoItem = processoDeCotacao.InformarCotacaoDeItem(cotacaoMaterialItemInformarVm.IdProcessoCotacaoItem,
                                                        cotacaoMaterialItemInformarVm.IdCotacao,
                                                        cotacaoMaterialItemInformarVm.ValorComImpostos.Value,
                                                        cotacaoMaterialItemInformarVm.Mva,
                                                        cotacaoMaterialItemInformarVm.QuantidadeDisponivel.Value,
                                                        Convert.ToDateTime(cotacaoMaterialItemInformarVm.PrazoDeEntrega),
                                                        cotacaoMaterialItemInformarVm.ObservacoesDoFornecedor);

                AtualizarImpostos(cotacaoItem, cotacaoMaterialItemInformarVm.Impostos);
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
using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AtualizadorDeCotacao : IAtualizadorDeCotacao
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IIncoterms _incoterms;
        private readonly ICondicoesDePagamento _condicoesDePagamento;

        public AtualizadorDeCotacao(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao,  
            IIncoterms incoterms, ICondicoesDePagamento condicoesDePagamento)
        {
            _processosDeCotacao = processosDeCotacao;
            _incoterms = incoterms;
            _condicoesDePagamento = condicoesDePagamento;
            _unitOfWork = unitOfWork;
        }

        public void Atualizar(CotacaoInformarVm cotacaoInformarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(cotacaoInformarVm.IdProcessoCotacao).Single();
                CondicaoDePagamento condicaoDePagamento = _condicoesDePagamento.BuscaPeloCodigo(cotacaoInformarVm.CodigoCondicaoPagamento);
                Incoterm incoterm = _incoterms.BuscaPeloCodigo(cotacaoInformarVm.CodigoIncoterm).Single();
                Cotacao cotacao = processoDeCotacao.InformarCotacao(cotacaoInformarVm.CodigoFornecedor,condicaoDePagamento, incoterm, 
                    cotacaoInformarVm.DescricaoIncoterm, cotacaoInformarVm.ValorComImpostos.Value,
                    cotacaoInformarVm.Mva, cotacaoInformarVm.QuantidadeDisponivel.Value, Convert.ToDateTime(cotacaoInformarVm.PrazoDeEntrega), 
                    cotacaoInformarVm.ObservacoesDoFornecedor);

                cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, cotacaoInformarVm.IcmsAliquota, cotacaoInformarVm.IcmsValor);
                cotacao.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, cotacaoInformarVm.IcmsStAliquota, cotacaoInformarVm.IcmsStValor);
                cotacao.InformarImposto(Enumeradores.TipoDeImposto.Ipi, cotacaoInformarVm.IpiAliquota, cotacaoInformarVm.IpiValor);
                cotacao.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, cotacaoInformarVm.PisCofinsAliquota,0);
                //cotacao.InformarImposto(Enumeradores.TipoDeImposto.Pis, cotacaoInformarVm.PisAliquota, cotacaoInformarVm.PisValor);
                //cotacao.InformarImposto(Enumeradores.TipoDeImposto.Cofins, cotacaoInformarVm.CofinsAliquota, cotacaoInformarVm.CofinsValor);

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
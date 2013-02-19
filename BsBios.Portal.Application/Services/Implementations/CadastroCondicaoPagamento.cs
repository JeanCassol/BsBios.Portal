using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroCondicaoPagamento: ICadastroCondicaoPagamento
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICondicoesDePagamento _condicoesDePagamento;
        private readonly ICadastroCondicaoPagamentoOperacao _cadastroCondicaoPagamentoOperacao;

        public CadastroCondicaoPagamento(IUnitOfWork unitOfWork, ICondicoesDePagamento condicoesDePagamento, ICadastroCondicaoPagamentoOperacao cadastroCondicaoPagamentoOperacao)
        {
            _unitOfWork = unitOfWork;
            _condicoesDePagamento = condicoesDePagamento;
            _cadastroCondicaoPagamentoOperacao = cadastroCondicaoPagamentoOperacao;
        }

        //public void Novo(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm)
        //{
        //    try
        //    {
        //        _unitOfWork.BeginTransaction();
        //        var condicaoDePagamento = new CondicaoDePagamento(condicaoDePagamentoCadastroVm.CodigoSap, condicaoDePagamentoCadastroVm.Descricao);
        //        _condicoesDePagamento.Save(condicaoDePagamento);                
        //        _unitOfWork.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        _unitOfWork.RollBack();                
        //        throw;
        //    }
        //}

        private void AtualizarCondicaoDePagamento(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm)
        {
            CondicaoDePagamento condicaoDePagamento = _condicoesDePagamento.BuscaPeloCodigoSap(condicaoDePagamentoCadastroVm.CodigoSap);
            if (condicaoDePagamento != null)
            {
                _cadastroCondicaoPagamentoOperacao.Alterar(condicaoDePagamento, condicaoDePagamentoCadastroVm);
            }
            else
            {
                _cadastroCondicaoPagamentoOperacao.Criar(condicaoDePagamentoCadastroVm);
            }
            _condicoesDePagamento.Save(condicaoDePagamento);
        }

        public void AtualizarCondicoesDePagamento(IList<CondicaoDePagamentoCadastroVm> condicoesDePagamento)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                foreach (var condicaoDePagamentoCadastroVm in condicoesDePagamento)
                {
                    AtualizarCondicaoDePagamento(condicaoDePagamentoCadastroVm);
                }
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

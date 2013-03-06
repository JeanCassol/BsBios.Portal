using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroCondicaoPagamento: ICadastroCondicaoPagamento
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICondicoesDePagamento _condicoesDePagamento;

        public CadastroCondicaoPagamento(IUnitOfWork unitOfWork, ICondicoesDePagamento condicoesDePagamento)
        {
            _unitOfWork = unitOfWork;
            _condicoesDePagamento = condicoesDePagamento;
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
            CondicaoDePagamento condicaoDePagamento = _condicoesDePagamento.BuscaPeloCodigo(condicaoDePagamentoCadastroVm.Codigo);
            if (condicaoDePagamento != null)
            {
                condicaoDePagamento.AtualizarDescricao(condicaoDePagamentoCadastroVm.Descricao);
            }
            else
            {
                condicaoDePagamento = new CondicaoDePagamento(condicaoDePagamentoCadastroVm.Codigo,
                                                              condicaoDePagamentoCadastroVm.Descricao);
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

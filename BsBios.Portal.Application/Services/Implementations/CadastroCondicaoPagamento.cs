﻿using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Model;
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

        public void Novo(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var condicaoDePagamento = new CondicaoDePagamento(condicaoDePagamentoCadastroVm.CodigoSap, condicaoDePagamentoCadastroVm.Descricao);
                _condicoesDePagamento.Save(condicaoDePagamento);                
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

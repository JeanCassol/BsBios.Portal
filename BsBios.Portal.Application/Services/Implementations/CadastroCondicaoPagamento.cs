using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroCondicaoPagamento: ICadastroCondicaoPagamento
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICondicoesDePagamento _condicoesDePagamento;
        private IList<CondicaoDePagamento> _condicoesDePagamentoConsultadas;

        public CadastroCondicaoPagamento(IUnitOfWork unitOfWork, ICondicoesDePagamento condicoesDePagamento)
        {
            _unitOfWork = unitOfWork;
            _condicoesDePagamento = condicoesDePagamento;
        }

        private void AtualizarCondicaoDePagamento(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm)
        {
            CondicaoDePagamento condicaoDePagamento =
                _condicoesDePagamentoConsultadas.SingleOrDefault(x => x.Codigo == condicaoDePagamentoCadastroVm.Codigo);
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
                _condicoesDePagamentoConsultadas =
                    _condicoesDePagamento.FiltraPorListaDeCodigos(condicoesDePagamento.Select(x => x.Codigo).ToArray())
                                         .List();
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

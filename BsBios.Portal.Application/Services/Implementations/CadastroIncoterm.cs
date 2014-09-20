using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroIncoterm:ICadastroIncoterm
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIncoterms _incoterms;
        private IList<Incoterm> _incotermsConsultados;
        public CadastroIncoterm(IUnitOfWork unitOfWork, IIncoterms incoterms)
        {
            _unitOfWork = unitOfWork;
            _incoterms = incoterms;
        }

        private void AtualizarIncoterm(IncotermCadastroVm incotermCadastroVm)
        {
            Incoterm incoterm = _incotermsConsultados.SingleOrDefault(x => x.Codigo == incotermCadastroVm.Codigo);
            if (incoterm != null)
            {
                incoterm.AtualizaDescricao(incotermCadastroVm.Descricao);
            }
            else
            {
                incoterm = new Incoterm(incotermCadastroVm.Codigo, incotermCadastroVm.Descricao);
            }
            _incoterms.Save(incoterm);
        }

        public  void AtualizarIncoterms(IList<IncotermCadastroVm> incoterms)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _incotermsConsultados = _incoterms.FiltraPorListaDeCodigos(incoterms.Select(x => x.Codigo).ToArray()).List();

                foreach (var incotermCadastroVm in incoterms)
                {
                    AtualizarIncoterm(incotermCadastroVm);
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

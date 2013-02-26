using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroIncoterm:ICadastroIncoterm
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIncoterms _incoterms;
        public CadastroIncoterm(IUnitOfWork unitOfWork, IIncoterms incoterms)
        {
            _unitOfWork = unitOfWork;
            _incoterms = incoterms;
        }

        private void AtualizarIncoterm(IncotermCadastroVm incotermCadastroVm)
        {
            Incoterm incoterm = _incoterms.BuscaPeloCodigo(incotermCadastroVm.Codigo).Single();
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

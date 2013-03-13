using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroIva:ICadastroIva
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIvas _ivas;
        private IList<Iva> _ivasConsultados;
        public CadastroIva(IUnitOfWork unitOfWork, IIvas ivas)
        {
            _unitOfWork = unitOfWork;
            _ivas = ivas;
        }

        private void AtualizarIva(IvaCadastroVm ivaCadastroVm)
        {
            Iva iva = _ivasConsultados.SingleOrDefault(x => x.Codigo == ivaCadastroVm.Codigo);
            if (iva != null)
            {
                iva.AtualizaDescricao(ivaCadastroVm.Descricao);
            }
            else
            {
                iva = new Iva(ivaCadastroVm.Codigo, ivaCadastroVm.Descricao);
            }
            _ivas.Save(iva);
        }

        public  void AtualizarIvas(IList<IvaCadastroVm> ivas)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _ivasConsultados = _ivas.BuscaListaPorCodigo(ivas.Select(x => x.Codigo).ToArray()).List();
                foreach (var ivaCadastroVm in ivas)
                {
                    AtualizarIva(ivaCadastroVm);
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

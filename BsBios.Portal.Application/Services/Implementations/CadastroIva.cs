using System;
using System.Collections.Generic;
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
        public CadastroIva(IUnitOfWork unitOfWork, IIvas ivas)
        {
            _unitOfWork = unitOfWork;
            _ivas = ivas;
        }

        //public void Novo(IvaCadastroVm ivaCadastroVm)
        //{
        //    try
        //    {
        //        _unitOfWork.BeginTransaction();
        //        var iva = new Iva(ivaCadastroVm.CodigoSap, ivaCadastroVm.Descricao);
        //        _ivas.Save(iva);
        //        _unitOfWork.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        _unitOfWork.RollBack();
        //        throw;
        //    }
        //}

        private void AtualizarIva(IvaCadastroVm ivaCadastroVm)
        {
            Iva iva = _ivas.BuscaPeloCodigo(ivaCadastroVm.Codigo);
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

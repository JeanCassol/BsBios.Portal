using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroIva:ICadastroIva
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIvas _ivas;
        private readonly ICadastroIvaOperacao _cadastroIvaOperacao;
        public CadastroIva(IUnitOfWork unitOfWork, IIvas ivas, ICadastroIvaOperacao cadastroIvaOperacao)
        {
            _unitOfWork = unitOfWork;
            _ivas = ivas;
            _cadastroIvaOperacao = cadastroIvaOperacao;
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
            Iva iva = _ivas.BuscaPeloCodigoSap(ivaCadastroVm.Codigo);
            if (iva != null)
            {
                _cadastroIvaOperacao.Alterar(iva,ivaCadastroVm);
            }
            else
            {
                iva = _cadastroIvaOperacao.Criar(ivaCadastroVm);
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

using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Model;
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

        public void Novo(IvaCadastroVm ivaCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var iva = new Iva(ivaCadastroVm.CodigoSap, ivaCadastroVm.Descricao);
                _ivas.Save(iva);
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

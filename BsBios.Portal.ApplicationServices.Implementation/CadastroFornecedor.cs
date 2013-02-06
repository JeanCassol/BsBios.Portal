using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Implementation
{
    public class CadastroFornecedor: ICadastroFornecedor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFornecedores _fornecedores;

        public CadastroFornecedor(IUnitOfWork unitOfWork, IFornecedores fornecedores)
        {
            _unitOfWork = unitOfWork;
            _fornecedores = fornecedores;
        }

        public void Novo(FornecedorCadastroVm fornecedorCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var fornecedor = new Fornecedor(fornecedorCadastroVm.CodigoSap, fornecedorCadastroVm.Nome);
                _fornecedores.Save(fornecedor);
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

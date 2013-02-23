using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
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
                var fornecedor = new Fornecedor(fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Nome,fornecedorCadastroVm.Email);
                _fornecedores.Save(fornecedor);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private void AtualizaFornecedor(FornecedorCadastroVm fornecedorCadastroVm)
        {
            Fornecedor fornecedor = _fornecedores.BuscaPeloCodigo(fornecedorCadastroVm.Codigo);

            if (fornecedor == null)
            {
                fornecedor = new Fornecedor(fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email);
            }
            else
            {
                fornecedor.Atualizar(fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email);
            }
                             
            _fornecedores.Save(fornecedor);
        }

        public void AtualizarFornecedores(IList<FornecedorCadastroVm> fornecedores)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                foreach (var fornecedorCadastroVm in fornecedores)
                {
                    AtualizaFornecedor(fornecedorCadastroVm);
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

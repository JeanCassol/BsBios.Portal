using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroFornecedor: ICadastroFornecedor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFornecedores _fornecedores;
        private readonly ICadastroFornecedorOperacao _cadastroFornecedorOperacao;

        public CadastroFornecedor(IUnitOfWork unitOfWork, IFornecedores fornecedores, ICadastroFornecedorOperacao cadastroFornecedorOperacao)
        {
            _unitOfWork = unitOfWork;
            _fornecedores = fornecedores;
            _cadastroFornecedorOperacao = cadastroFornecedorOperacao;
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
                fornecedor = _cadastroFornecedorOperacao.Criar(fornecedorCadastroVm);
            }
            else
            {
                _cadastroFornecedorOperacao.Atualizar(fornecedor,fornecedorCadastroVm);
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

using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroFornecedor: ICadastroFornecedor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFornecedores _fornecedores;
        private readonly IUsuarios _usuarios;

        public CadastroFornecedor(IUnitOfWork unitOfWork, IFornecedores fornecedores, IUsuarios usuarios)
        {
            _unitOfWork = unitOfWork;
            _fornecedores = fornecedores;
            _usuarios = usuarios;
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
            Usuario usuario = null;

            if (fornecedor == null)
            {
                fornecedor = new Fornecedor(fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email);
                usuario = new Usuario(fornecedorCadastroVm.Nome, fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Email);
                usuario.AdicionarPerfil(Enumeradores.Perfil.Fornecedor);
            }
            else
            {
                fornecedor.Atualizar(fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email);
            }
                             
            _fornecedores.Save(fornecedor);
            if (usuario != null)
            {
                _usuarios.Save(usuario);
            }
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

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
        private IList<Fornecedor> _fornecedoresConsultados;
        private IList<Usuario> _usuariosConsultados;

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
                var fornecedor = new Fornecedor(fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Nome,fornecedorCadastroVm.Email,
                    fornecedorCadastroVm.Cnpj, fornecedorCadastroVm.Municipio, fornecedorCadastroVm.Uf, 
                    fornecedorCadastroVm.ToString().ToLower().Equals("x"));
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
            Fornecedor fornecedor = _fornecedoresConsultados.SingleOrDefault(x => x.Codigo == fornecedorCadastroVm.Codigo);

            if (fornecedor == null)
            {
                fornecedor = new Fornecedor(fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email,
                    fornecedorCadastroVm.Cnpj, fornecedorCadastroVm.Municipio, fornecedorCadastroVm.Uf,
                    fornecedorCadastroVm.Transportadora.ToLower().Equals("x"));
            }
            else
            {
                fornecedor.Atualizar(fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email, fornecedorCadastroVm.Cnpj,
                    fornecedorCadastroVm.Municipio, fornecedorCadastroVm.Uf, fornecedorCadastroVm.Transportadora.ToLower().Equals("x"));
            }
                             
            _fornecedores.Save(fornecedor);


            Usuario usuario = _usuariosConsultados.SingleOrDefault(x => x.Login == fornecedorCadastroVm.Codigo);
            if (usuario == null)
            {
                usuario = new Usuario(fornecedorCadastroVm.Nome, fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Email);
                usuario.AdicionarPerfil(Enumeradores.Perfil.Fornecedor);
            }
            else
            {
                usuario.Alterar(fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email);
            }
            _usuarios.Save(usuario);
        }

        public void AtualizarFornecedores(IList<FornecedorCadastroVm> fornecedores)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var codigosDosFornecedores = fornecedores.Select(x => x.Codigo).ToArray();
                _fornecedoresConsultados = _fornecedores.BuscaListaPorCodigo(codigosDosFornecedores).List();
                _usuariosConsultados = _usuarios.FiltraPorListaDeLogins(codigosDosFornecedores).List();
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

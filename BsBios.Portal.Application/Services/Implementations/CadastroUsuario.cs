using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroUsuario: ICadastroUsuario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarios _usuarios;
        private readonly IProvedorDeCriptografia _provedorDeCriptografia;
        private readonly ICadastroUsuarioOperacao _cadastroUsuarioOperacao;

        public CadastroUsuario(IUnitOfWork unitOfWork, IUsuarios usuarios, IProvedorDeCriptografia provedorDeCriptografia
            , ICadastroUsuarioOperacao cadastroUsuarioOperacao)
        {
            _unitOfWork = unitOfWork;
            _usuarios = usuarios;
            _provedorDeCriptografia = provedorDeCriptografia;
            _cadastroUsuarioOperacao = cadastroUsuarioOperacao;
        }

        public void Novo(UsuarioCadastroVm usuarioVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                string senhaCriptografada = _provedorDeCriptografia.Criptografar("123");
                var novoUsuario = new Usuario(usuarioVm.Nome, usuarioVm.Login, senhaCriptografada
                    , usuarioVm.Email, Enumeradores.Perfil.Comprador);
                _usuarios.Save(novoUsuario);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private void AtualizarUsuario(UsuarioCadastroVm usuarioCadastroVm)
        {
            Usuario usuario = _usuarios.BuscaPorLogin(usuarioCadastroVm.Login);
            if (usuario != null)
            {
                _cadastroUsuarioOperacao.Alterar(usuario,usuarioCadastroVm);
            }
            else
            {
                string senhaCriptografada = _provedorDeCriptografia.Criptografar("123");
                usuario = _cadastroUsuarioOperacao.Criar(usuarioCadastroVm, senhaCriptografada);
            }
            _usuarios.Save(usuario);
        }

        public void AtualizarUsuarios(IList<UsuarioCadastroVm> usuarios)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                foreach (var usuarioCadastroVm in usuarios)
                {
                    AtualizarUsuario(usuarioCadastroVm);
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

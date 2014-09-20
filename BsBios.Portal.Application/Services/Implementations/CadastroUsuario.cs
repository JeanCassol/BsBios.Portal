using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroUsuario: ICadastroUsuario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarios _usuarios;

        public CadastroUsuario(IUnitOfWork unitOfWork, IUsuarios usuarios)
        {
            _unitOfWork = unitOfWork;
            _usuarios = usuarios;
        }

        public void Novo(UsuarioCadastroVm usuarioVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var novoUsuario = new Usuario(usuarioVm.Nome, usuarioVm.Login
                    , usuarioVm.Email);
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
                usuario.Alterar(usuarioCadastroVm.Nome, usuarioCadastroVm.Email);
            }
            else
            {
                usuario = new Usuario(usuarioCadastroVm.Nome, usuarioCadastroVm.Login,
                                      usuarioCadastroVm.Email);
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

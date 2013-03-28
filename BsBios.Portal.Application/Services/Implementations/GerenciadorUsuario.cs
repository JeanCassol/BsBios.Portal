using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class GerenciadorUsuario : IGerenciadorUsuario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarios _usuarios;
        private readonly IProvedorDeCriptografia _provedorDeCriptografia;
        private readonly IGeradorDeSenha _geradorDeSenha;
        private readonly IBuilder<Usuario, UsuarioConsultaVm> _builder;
        private readonly IGeradorDeEmail _geradorDeEmail;

        public GerenciadorUsuario(IUnitOfWork unitOfWork, IUsuarios usuarios, IProvedorDeCriptografia provedorDeCriptografia, 
            IGeradorDeSenha geradorDeSenha, IBuilder<Usuario, UsuarioConsultaVm> builder, IGeradorDeEmail geradorDeEmail)
        {
            _unitOfWork = unitOfWork;
            _usuarios = usuarios;
            _provedorDeCriptografia = provedorDeCriptografia;
            _geradorDeSenha = geradorDeSenha;
            _builder = builder;
            _geradorDeEmail = geradorDeEmail;
        }

        public UsuarioConsultaVm CriarSenha(string login)
        {
            try
            {
                string senha = _geradorDeSenha.GerarGuid(8);
                _unitOfWork.BeginTransaction();
                Usuario usuario = _usuarios.BuscaPorLogin(login);
                if (usuario == null)
                {
                    throw  new UsuarioNaoCadastradoException(login);
                }
                _geradorDeEmail.CriacaoAutomaticaDeSenha(usuario, senha);

                string senhaCriptografada = _provedorDeCriptografia.Criptografar(senha);
                usuario.CriarSenha(senhaCriptografada);
                _usuarios.Save(usuario);

                UsuarioConsultaVm vm = _builder.BuildSingle(usuario);

                _unitOfWork.Commit();

                return vm;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();   
                throw;
            }
        }

        public void AlterarSenha(string login, string senhaAtual, string senhaNova)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Usuario usuario = _usuarios.BuscaPorLogin(login);
                if (usuario == null)
                {
                    throw new UsuarioNaoCadastradoException(login);
                }
                string senhaAtualCriptografada = _provedorDeCriptografia.Criptografar(senhaAtual);
                string senhaNovaCriptografada = _provedorDeCriptografia.Criptografar(senhaNova);
                usuario.AlterarSenha(senhaAtualCriptografada, senhaNovaCriptografada);
                _usuarios.Save(usuario);

                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void Ativar(string login)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Usuario usuario = _usuarios.BuscaPorLogin(login);
                usuario.Ativar();
                _usuarios.Save(usuario);
                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void Bloquear(string login)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Usuario usuario = _usuarios.BuscaPorLogin(login);
                usuario.Bloquear();
                _usuarios.Save(usuario);

                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void AtualizarPerfis(string login, IList<Enumeradores.Perfil> perfis)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Usuario usuario = _usuarios.BuscaPorLogin(login);
                IList<Enumeradores.Perfil> perfisRemovidos = usuario.Perfis.Except(perfis).ToList();
                IList<Enumeradores.Perfil> perfisParaAdicionar = perfis.Except(usuario.Perfis).ToList();
                foreach (var perfilRemovido in perfisRemovidos)
                {
                    usuario.RemoverPerfil(perfilRemovido);
                }

                foreach (var perfilParaAdicionar in perfisParaAdicionar)
                {
                    usuario.AdicionarPerfil(perfilParaAdicionar);
                }
                _usuarios.Save(usuario);

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
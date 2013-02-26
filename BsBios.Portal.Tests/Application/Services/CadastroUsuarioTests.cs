using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class CadastroUsuarioTests
    {
        private readonly Mock<IProvedorDeCriptografia> _provedorDeCriptografiaMock;
        private readonly Mock<IUsuarios> _usuariosMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock; 
        private readonly ICadastroUsuario _cadastroUsuario;
        private readonly UsuarioCadastroVm _usuarioPadrao;


        public CadastroUsuarioTests()
        {
            _provedorDeCriptografiaMock = new Mock<IProvedorDeCriptografia>(MockBehavior.Strict);
            _provedorDeCriptografiaMock.Setup(x => x.Criptografar(It.IsAny<string>())).Returns("criptografado");

            _usuariosMock = new Mock<IUsuarios>(MockBehavior.Strict);
            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()))
                .Callback((Usuario usuario) => Assert.IsNotNull(usuario) );
            _usuariosMock.Setup(x => x.BuscaPorLogin(It.IsAny<string>()))
                         .Returns(
                             (string login) =>
                             login == "USER001"
                                 ? new UsuarioParaAtualizacao("USUARIO 001", "USER001", "", Enumeradores.Perfil.Comprador)
                                  : null);

            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();

            _cadastroUsuario = new CadastroUsuario(_unitOfWorkMock.Object, _usuariosMock.Object,_provedorDeCriptografiaMock.Object);

            _usuarioPadrao = new UsuarioCadastroVm()
                {
                    Nome = "Mauro Leal",
                    Login =  "mauroscl",
                    //Senha = "123" ,
                    Email = "mauro.leal@fusionconsultoria.com.br" ,
                    //CodigoPerfil = 1
                };


        }

        #region cadastro de usuários


        [TestMethod]
        public void QuandoCadastroUmaListaDeUsuariosEPersistidoNoBanco()
        {
            var usuarios = new List<UsuarioCadastroVm>()
                {
                    new UsuarioCadastroVm()
                        {
                            Nome = "USUARIO 001",
                            Login =  "USER001",
                            Email = "usuario001@fusionconsultoria.com.br" ,
                        },
                    new UsuarioCadastroVm()
                        {
                            Nome = "USUARIO 002",
                            Login =  "USER002",
                            Email = "usuario002@fusionconsultoria.com.br" ,
                        }
                };
            _cadastroUsuario.AtualizarUsuarios(usuarios);
            _usuariosMock.Verify(x=> x.Save(It.IsAny<Usuario>()), Times.Exactly(usuarios.Count));
        }

        [TestMethod]
        public void QuandoCadastroUmNovoUsuarioExisteControleDeTransacao()
        {
            _cadastroUsuario.Novo(_usuarioPadrao);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
        }

        [TestMethod]
        public void QuandoOcorreAlgumaExcecaoFazRollback()
        {
            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>())).Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o usuário"));
            try
            {
                _cadastroUsuario.Novo(_usuarioPadrao);
            }
            catch(ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
            }
        }

        [TestMethod]
        public void QuandoReceberUmUsuarioExistenteDeveAtualizar()
        {
            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()))
                .Callback((Usuario usuario) =>
                    {
                        Assert.IsNotNull(usuario);
                        Assert.IsInstanceOfType(usuario, typeof(UsuarioParaAtualizacao));
                        Assert.AreEqual("USER001", usuario.Login);
                        Assert.AreEqual("USUARIO 001 ALTERADO", usuario.Nome);
                        Assert.AreEqual("user001@empresa.com.br", usuario.Email);
                    });

            _cadastroUsuario.AtualizarUsuarios(new List<UsuarioCadastroVm>()
                {
                    new UsuarioCadastroVm()
                        {
                            Login = "USER001" ,
                            Nome = "USUARIO 001 ALTERADO" ,
                            Email = "user001@empresa.com.br"
                        }
                });

        }
        [TestMethod]
        public void QuandoReceberUmUsuarioNovoDeveAdicionar()
        {
            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()))
                .Callback((Usuario usuario) =>
                {
                    Assert.IsNotNull(usuario);
                    Assert.IsNotInstanceOfType(usuario, typeof(UsuarioParaAtualizacao));
                    Assert.AreEqual("USER002", usuario.Login);
                    Assert.AreEqual("USUARIO 002", usuario.Nome);
                    Assert.AreEqual("user002@empresa.com.br", usuario.Email);
                });

            _cadastroUsuario.AtualizarUsuarios(new List<UsuarioCadastroVm>()
                {
                    new UsuarioCadastroVm()
                        {
                            Login = "USER002" ,
                            Nome = "USUARIO 002" ,
                            Email = "user002@empresa.com.br"
                        }
                });

        }

        #endregion

        #region criar senha
        [TestMethod]
        public void UmaNovaSenhaCriadaParaOUsarioECriptografada()
        {
            Assert.Fail();
        }
        #endregion


    }

    public class UsuarioParaAtualizacao: Usuario
    {
        public UsuarioParaAtualizacao(string nome, string login, string email, Enumeradores.Perfil perfil) : base(nome, login, email, perfil)
        {
        }
    }
}

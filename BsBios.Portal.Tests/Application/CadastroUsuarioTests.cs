using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application
{
    [TestClass]
    public class CadastroUsuarioTests
    {
        private readonly Mock<IProvedorDeCriptografia> _provedorDeCriptografiaMock;
        private readonly Mock<IUsuarios> _usuariosMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock; 
        private readonly ICadastroUsuario _cadastroUsuario;
        private readonly UsuarioCadastroVm _usuarioPadrao;
        private readonly Mock<ICadastroUsuarioOperacao> _cadastroUsuarioOperacaoMock;


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
                                 ? new Usuario("USUARIO 001", "USER001", "", Enumeradores.Perfil.Comprador)
                                 : null);

            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();

            _cadastroUsuarioOperacaoMock = new Mock<ICadastroUsuarioOperacao>(MockBehavior.Strict);
            _cadastroUsuarioOperacaoMock.Setup(x => x.Criar(It.IsAny<UsuarioCadastroVm>()))
                                    .Returns(new Usuario("USER0001", "user", "user@empresa.com.br", Enumeradores.Perfil.Comprador));
            _cadastroUsuarioOperacaoMock.Setup(x => x.Alterar(It.IsAny<Usuario>(), It.IsAny<UsuarioCadastroVm>()));

            _cadastroUsuario = new CadastroUsuario(_unitOfWorkMock.Object, _usuariosMock.Object,_provedorDeCriptografiaMock.Object,_cadastroUsuarioOperacaoMock.Object);

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
            _cadastroUsuario.AtualizarUsuarios(new List<UsuarioCadastroVm>()
                {
                    new UsuarioCadastroVm()
                        {
                            Login = "USER001" ,
                            Nome = "USUARIO 001" ,
                            Email = "user001@empresa.com.br"
                        }
                });

            _cadastroUsuarioOperacaoMock.Verify(x => x.Criar(It.IsAny<UsuarioCadastroVm>()),Times.Never());
            _cadastroUsuarioOperacaoMock.Verify(x => x.Alterar(It.IsAny<Usuario>(), It.IsAny<UsuarioCadastroVm>()), Times.Once());

        }
        [TestMethod]
        public void QuandoReceberUmUsuarioNovoDeveAdicionar()
        {
            _cadastroUsuario.AtualizarUsuarios(new List<UsuarioCadastroVm>()
                {
                    new UsuarioCadastroVm()
                        {
                            Login = "USER002" ,
                            Nome = "USUARIO 002" ,
                            Email = "user002@empresa.com.br"
                        }
                });

            _cadastroUsuarioOperacaoMock.Verify(x => x.Criar(It.IsAny<UsuarioCadastroVm>()), Times.Once());
            _cadastroUsuarioOperacaoMock.Verify(x => x.Alterar(It.IsAny<Usuario>(), It.IsAny<UsuarioCadastroVm>()), Times.Never());

        }

        #endregion

        #region criar senha
        [TestMethod]
        public void AoCriarUmaSenhaPara
        #endregion


    }
}

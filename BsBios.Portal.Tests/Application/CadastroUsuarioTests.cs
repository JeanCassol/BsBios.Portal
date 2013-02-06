using System;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.ApplicationServices.Implementation;
using BsBios.Portal.Domain.Model;
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
        private readonly Mock<IUnitOfWorkNh> _unitOfWorkNhMock; 
        private readonly ICadastroUsuario _cadastroUsuario;
        private readonly UsuarioVm _usuarioPadrao;


        public CadastroUsuarioTests()
        {
            _provedorDeCriptografiaMock = new Mock<IProvedorDeCriptografia>(MockBehavior.Strict);
            _provedorDeCriptografiaMock.Setup(x => x.Criptografar(It.IsAny<string>())).Returns("criptografado");

            _usuariosMock = new Mock<IUsuarios>(MockBehavior.Strict);
            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()));

            _unitOfWorkNhMock = DefaultRepository.GetDefaultMockUnitOfWork();

            _cadastroUsuario = new CadastroUsuario(_unitOfWorkNhMock.Object, _usuariosMock.Object,_provedorDeCriptografiaMock.Object);

            _usuarioPadrao = new UsuarioVm()
                {
                    Nome = "Mauro Leal",
                    Login =  "mauroscl",
                    Senha = "123" ,
                    Email = "mauro.leal@fusionconsultoria.com.br" ,
                    CodigoPerfil = 1
                };
        }

        

        [TestMethod]
        public void QuandoCadastroUmNovoUsuarioASenhaEcriptografada()
        {
            _cadastroUsuario.Novo(_usuarioPadrao);
            _provedorDeCriptografiaMock.Verify(x => x.Criptografar(It.IsAny<string>()),Times.Once());
               
        }

        [TestMethod]
        public void QuandoCadastroUmNovoUsuarioEPersistidoNoBanco()
        {
            _cadastroUsuario.Novo(_usuarioPadrao);
            _usuariosMock.Verify(x=> x.Save(It.IsAny<Usuario>()), Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmNovoUsuarioExisteControleDeTransacao()
        {
            _cadastroUsuario.Novo(_usuarioPadrao);
            _unitOfWorkNhMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkNhMock.Verify(x => x.Commit(), Times.Once());
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
                _unitOfWorkNhMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkNhMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkNhMock.Verify(x => x.Commit(), Times.Never());
            }
        }
    }
}

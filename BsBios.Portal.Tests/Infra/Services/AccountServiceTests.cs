using System;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class AccountServiceTests
    {
        private readonly Mock<IAuthenticationProvider> _authenticationProviderMock;
        private readonly Mock<IValidadorUsuario> _validadorUsuarioMock;
        //private readonly IValidadorUsuario _validadorUsuario;

        public AccountServiceTests()
        {
            _authenticationProviderMock = new Mock<IAuthenticationProvider>(MockBehavior.Strict);
            _authenticationProviderMock.Setup(x => x.Autenticar(It.IsAny<UsuarioConectado>()));
            //_validadorUsuario = new ValidadorUsuario();
            _validadorUsuarioMock = new Mock<IValidadorUsuario>(MockBehavior.Strict);
            _validadorUsuarioMock.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns((string u, string s) =>
                                     {
                                         if (u == "comprador" && s == "123")
                                         {
                                             return new UsuarioConectado("user001", "Comprador", 1);
                                         }
                                         else
                                         {
                                             throw new UsuarioNaoCadastradoException(u);
                                         }
                                     }
                );
        }

        [TestMethod]
        public void ConsigoInstanciarOServiceViaIoC()
        {
            var servico = ObjectFactory.GetInstance<IAccountService>();
            Assert.IsInstanceOfType(servico, typeof(AccountService));
        }

        [TestMethod]
        public void QuandoLogarComUmUsuarioValidoDeveAutenticar()
        {

            //var validadorUsuarioMock = new Mock<IValidadorUsuario>(MockBehavior.Strict);
            //validadorUsuarioMock.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<string>()))
            //                    .Returns(new UsuarioConectado("comprador", new PerfilComprador()));
            
            
            var accountService = new AccountService(_authenticationProviderMock.Object, _validadorUsuarioMock.Object);

            accountService.Login("comprador", "123");

            _authenticationProviderMock.Verify(x => x.Autenticar(It.IsAny<UsuarioConectado>()),Times.Once());

        }

        [TestMethod]
        public void QuandoLogarComUsuarioInvalidoDeveGerarExcecao()
        {
            //var validadorUsuarioMock = new Mock<IValidadorUsuario>(MockBehavior.Strict);
            //validadorUsuarioMock.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<string>()))
            //                    .Returns(new UsuarioConectado("naoautorizado", new PerfilNaoAutorizado()));
            
            var accountService = new AccountService(_authenticationProviderMock.Object, _validadorUsuarioMock.Object);

            try
            {
                accountService.Login("naoautorizado", "123");
                Assert.Fail("Deveria ter gerado exceção");

            }
            catch (UsuarioNaoCadastradoException)
            {
                _authenticationProviderMock.Verify(x => x.Autenticar(It.IsAny<UsuarioConectado>()), Times.Never());
            }


            

            
        }



    }
}

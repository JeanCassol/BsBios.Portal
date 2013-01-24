using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.InfraServices
{
    [TestClass]
    public class AccountServiceTests
    {
        private readonly Mock<IAuthenticationProvider> _authenticationProviderMock;
        private readonly IValidadorUsuario _validadorUsuario;

        public AccountServiceTests()
        {
            _authenticationProviderMock = new Mock<IAuthenticationProvider>(MockBehavior.Strict);
            _authenticationProviderMock.Setup(x => x.Autenticar(It.IsAny<UsuarioConectado>()));
            _validadorUsuario = new ValidadorUsuario();
        }

        [TestMethod]
        public void QuandoLogarComUmUsuarioValidoDeveAutenticar()
        {

            //var validadorUsuarioMock = new Mock<IValidadorUsuario>(MockBehavior.Strict);
            //validadorUsuarioMock.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<string>()))
            //                    .Returns(new UsuarioConectado("comprador", new PerfilComprador()));
            
            var accountService = new AccountService(_authenticationProviderMock.Object, _validadorUsuario);

            accountService.Login("comprador", "123");

            _authenticationProviderMock.Verify(x => x.Autenticar(It.IsAny<UsuarioConectado>()),Times.Once());

        }

        [TestMethod]
        public void QuandoLogarComUsuarioInvalidoNaoDeveAutenticar()
        {
            //var validadorUsuarioMock = new Mock<IValidadorUsuario>(MockBehavior.Strict);
            //validadorUsuarioMock.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<string>()))
            //                    .Returns(new UsuarioConectado("naoautorizado", new PerfilNaoAutorizado()));
            
            var accountService = new AccountService(_authenticationProviderMock.Object, _validadorUsuario);

            accountService.Login("naoautorizado", "123");

            _authenticationProviderMock.Verify(x => x.Autenticar(It.IsAny<UsuarioConectado>()), Times.Never());

            

            
        }



    }
}

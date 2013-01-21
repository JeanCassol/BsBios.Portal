using System.Web.Mvc;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.UI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestInitialize]
        public void Inicializar()
        {
            //_accountController = new AccountController(ObjectFactory.GetInstance<IAuthenticationProvider>());
        }

        //private AccountController _accountController;

        [TestMethod]
        public void QuandoLogarComCredenciaisValidasDeveRedirecionarParaPaginaInicial()
        {
            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(new UsuarioConectado("comprador",new PerfilComprador()));
            var accountController = new AccountController(accountServiceMock.Object);

            var result = (RedirectToRouteResult) accountController.Login(new LoginModel(){UserName = "comprador",Password = "123"});

            Assert.AreEqual("Home",result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void QuandoLogarComCredenciaisInvalidasDeveAdicionarErroNaModelState()
        {
            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new UsuarioConectado("nao autorizado",new PerfilNaoAutorizado()));
            var accountController = new AccountController(accountServiceMock.Object);

            accountController.Login(new LoginModel() { UserName = "comprador", Password = "1234" });
            Assert.IsFalse(accountController.ModelState.IsValid);
        }

        [TestMethod]
        public void QuandoLogarComCredenciaisInvalidasDeveRetonrarParaViewDeLogin()
        {

            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new UsuarioConectado("nao autorizado",new PerfilNaoAutorizado()));
            var accountController = new AccountController(accountServiceMock.Object);

            var result = (ViewResult)accountController.Login(new LoginModel() { UserName = "comprador", Password = "1234" });
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(LoginModel));

        }
    }
}

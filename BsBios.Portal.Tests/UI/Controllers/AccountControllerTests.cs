using System.Web.Mvc;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
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
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(new UsuarioConectado(1,"comprador",1));
            var accountController = new AccountController(accountServiceMock.Object);

            var result = (RedirectToRouteResult) accountController.Login(new LoginVm(){Usuario  = "comprador",Senha = "123"});

            Assert.AreEqual("Home",result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void QuandoLogarComCredenciaisInvalidasDeveAdicionarErroNaModelState()
        {
            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                              .Throws(new UsuarioNaoCadastradoException("comprador"));
                //.Returns(new UsuarioConectado(0,"nao autorizado",0));
            var accountController = new AccountController(accountServiceMock.Object);

            accountController.Login(new LoginVm() { Usuario = "comprador", Senha = "1234" });
            Assert.IsFalse(accountController.ModelState.IsValid);
        }

        [TestMethod]
        public void QuandoLogarComCredenciaisInvalidasDeveRetornarParaViewDeLogin()
        {

            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new UsuarioNaoCadastradoException("comprador"));
                //.Returns(new UsuarioConectado(0, "nao autorizado",0));
            var accountController = new AccountController(accountServiceMock.Object);

            var result = (ViewResult)accountController.Login(new LoginVm() { Usuario = "comprador", Senha = "1234" });
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(LoginVm));

        }
    }


}

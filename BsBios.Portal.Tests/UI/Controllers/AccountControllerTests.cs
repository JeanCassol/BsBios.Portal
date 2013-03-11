using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void QuandoLogarComCredenciaisValidasDeveRedirecionarParaPaginaInicial()
        {
            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(new UsuarioConectado("user001","comprador",new List<Enumeradores.Perfil>()));
            var accountController = new AccountController(accountServiceMock.Object);

            var result = (RedirectToRouteResult) accountController.Login(new LoginVm(){Usuario  = "comprador",Senha = "123"},"");

            Assert.AreEqual("Home",result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void QuandoLogarComUrlDeRetornoDeveRetornarParaUrlDeRetorno()
        {
            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(new UsuarioConectado("user001", "comprador", new List<Enumeradores.Perfil>()));
            var accountController = new AccountController(accountServiceMock.Object);

            CommonMocks.MockControllerUrl(accountController);

            var result = (RedirectResult)  accountController.Login(new LoginVm() { Usuario = "comprador", Senha = "123" }, "/Produto/Index");

            Assert.IsNotNull(result);
            Assert.AreEqual("/Produto/Index", result.Url);
        }

        [TestMethod]
        public void QuandoLogarComCredenciaisInvalidasDeveAdicionarErroNaModelState()
        {
            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                              .Throws(new UsuarioNaoCadastradoException("comprador"));
                //.Returns(new UsuarioConectado(0,"nao autorizado",0));
            var accountController = new AccountController(accountServiceMock.Object);

            accountController.Login(new LoginVm() { Usuario = "comprador", Senha = "1234" },"");
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

            var result = (ViewResult)accountController.Login(new LoginVm() { Usuario = "comprador", Senha = "1234" },"");
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(LoginVm));

        }

        [TestMethod]
        public void QuandoDesconectarRetornaParaActionDeLogin()
        {
            var accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
            accountServiceMock.Setup(x => x.Logout());

            var accountController = new AccountController(accountServiceMock.Object);
            var result = (RedirectToRouteResult) accountController.LogOff();
            accountServiceMock.Verify(x => x.Logout(), Times.Once());
            Assert.AreEqual("Account", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }


}

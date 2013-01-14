using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.UI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestInitialize]
        public void Inicializar()
        {
            _accountController = new AccountController(ObjectFactory.GetInstance<IAuthenticationProvider>());
        }

        private AccountController _accountController;

        [TestMethod]
        public void QuandoLogarComCredenciaisValidasDeveRedirecionarParaPaginaInicial()
        {
            var result = (RedirectToRouteResult) _accountController.Login(new LoginModel(){UserName = "comprador",Password = "123"});
            Assert.AreEqual("Home",result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void QuandoLogarComCredenciaisInvalidasDeveAdicionarErroNaModelState()
        {
            _accountController.Login(new LoginModel() { UserName = "comprador", Password = "1234" });
            Assert.IsFalse(_accountController.ModelState.IsValid);
        }

        [TestMethod]
        public void QuandoLogarComCredenciaisInvalidasDeveRetonrarParaViewDeLogin()
        {
            var result = (ViewResult)_accountController.Login(new LoginModel() { UserName = "comprador", Password = "1234" });
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(LoginModel));
        }
    }
}

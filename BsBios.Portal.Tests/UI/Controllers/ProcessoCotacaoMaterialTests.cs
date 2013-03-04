using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProcessoCotacaoMaterialTests
    {
        [TestMethod]
        public void QuandoAConsultaParaEditarOCadastroForExecutadaComSucessoDeveExibirAViewSemErros()
        {
            var consultaProcessoCotacaoMaterialMock = new Mock<IConsultaProcessoDeCotacaoDeMaterial>(MockBehavior.Strict);
            consultaProcessoCotacaoMaterialMock.Setup(x => x.ConsultaProcesso(It.IsAny<int>()))
                                               .Returns(new ProcessoCotacaoMaterialCadastroVm());
            var controller = new ProcessoCotacaoMaterialController(consultaProcessoCotacaoMaterialMock.Object);
            ViewResult  viewResult = controller.EditarCadastro(10);
            Assert.AreEqual("Cadastro", viewResult.ViewName);
            Assert.IsNull(controller.ViewData["erro"]);
        }
        [TestMethod]
        public void QuandoAConsultaParaEditarOCadastroForExecutadaComSucessoDeveExibirAViewComErros()
        {
            var consultaProcessoCotacaoMaterialMock = new Mock<IConsultaProcessoDeCotacaoDeMaterial>(MockBehavior.Strict);
            consultaProcessoCotacaoMaterialMock.Setup(x => x.ConsultaProcesso(It.IsAny<int>()))
                                               .Throws(new ExcecaoDeTeste("Erro ao consultar Processo"));
            var controller = new ProcessoCotacaoMaterialController(consultaProcessoCotacaoMaterialMock.Object);
            ViewResult viewResult = controller.EditarCadastro(10);
            Assert.AreEqual("Cadastro", viewResult.ViewName);
            Assert.IsNotNull(controller.ViewData["erro"]);
            Assert.AreEqual("Erro ao consultar Processo", controller.ViewData["erro"]);
        }
    }
}

using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProcessoDeCotacaoServiceControllerTests
    {
        [TestMethod]
        public void QuandoAtualizaOProcessoComSucessoDeveIrParaPaginaDeListagem()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()));

            var processoDeCotacaoController = new ProcessoDeCotacaoServiceController(processoDeCotacaoServiceMock.Object);
            var redirectResult = (RedirectToRouteResult) processoDeCotacaoController.AtualizarProcesso(new ProcessoDeCotacaoAtualizarVm());

            Assert.AreEqual("ProcessoCotacaoMaterial", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            
            processoDeCotacaoServiceMock.Verify(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorreErroAoAtualizarProcessoRetornarParaPaginaDeCadastro()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()))
                .Throws(new ExcecaoDeTeste("Erro ao Atualizar Processo de Cotação"));

            var processoDeCotacaoController = new ProcessoDeCotacaoServiceController(processoDeCotacaoServiceMock.Object);
            var redirectResult = (RedirectToRouteResult)processoDeCotacaoController.AtualizarProcesso(
                new ProcessoDeCotacaoAtualizarVm()
                    {
                        Id = 10,
                        DataLimiteRetorno = DateTime.Today
                    });

            Assert.AreEqual("ProcessoCotacaoMaterial", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("EditarCadastro", redirectResult.RouteValues["action"]);
            Assert.AreEqual(10, redirectResult.RouteValues["idProcessoCotacao"]); 
            Assert.IsNotNull(processoDeCotacaoController.ViewData["erro"]);
            Assert.AreEqual("Erro ao Atualizar Processo de Cotação", processoDeCotacaoController.ViewData["erro"]);

            processoDeCotacaoServiceMock.Verify(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()), Times.Once());
            
        }

    }
}

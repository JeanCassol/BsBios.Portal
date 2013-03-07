using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
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
        private readonly Mock<IConsultaProcessoDeCotacaoDeMaterial> _consultaProcessoCotacaoMaterialMock;
        private readonly Mock<IConsultaIva> _consultaIvaMock;
        private readonly ProcessoCotacaoMaterialController _controller;

        public ProcessoCotacaoMaterialTests()
        {
            _consultaProcessoCotacaoMaterialMock = new Mock<IConsultaProcessoDeCotacaoDeMaterial>(MockBehavior.Strict);
            _consultaProcessoCotacaoMaterialMock.Setup(x => x.ConsultaProcesso(It.IsAny<int>()))
                                               .Returns(new ProcessoCotacaoMaterialCadastroVm());
            _consultaIvaMock = new Mock<IConsultaIva>(MockBehavior.Strict);
            _controller = new ProcessoCotacaoMaterialController(_consultaProcessoCotacaoMaterialMock.Object, _consultaIvaMock.Object);
            CommonMocks.MockControllerUrl(_controller);
        }

        [TestMethod]
        public void QuandoAConsultaParaEditarOCadastroForExecutadaComSucessoDeveExibirAViewSemErros()
        {
            ViewResult  viewResult = _controller.EditarCadastro(10);
            Assert.AreEqual("Cadastro", viewResult.ViewName);
            Assert.IsNull(_controller.ViewData["erro"]);

        }
        [TestMethod]
        public void QuandoAConsultaParaEditarOCadastroForExecutadaComSucessoDeveExibirAViewComErros()
        {
            _consultaProcessoCotacaoMaterialMock.Setup(x => x.ConsultaProcesso(It.IsAny<int>()))
                                               .Throws(new ExcecaoDeTeste("Erro ao consultar Processo"));
            ViewResult viewResult = _controller.EditarCadastro(10);
            Assert.AreEqual("Cadastro", viewResult.ViewName);
            Assert.IsNotNull(_controller.ViewData["erro"]);
            Assert.AreEqual("Erro ao consultar Processo", _controller.ViewData["erro"]);
        }

        [TestMethod]
        public void QuandoChamaActionParaListarProcessosRetornaAViewEsperada()
        {
            var viewResult = (ViewResult) _controller.Index();
            Assert.AreEqual("", viewResult.ViewName);
        }
        [TestMethod]
        public void QuandoEstaConectadoComOPerfilCompradorViewBagContemActionDeEdicaoEsperada()
        {
            //_usuarioConectado = new UsuarioConectado("comprador", "Usuário Comprador", 1);
            _controller.Index();
            Assert.IsNotNull(_controller.ViewData["ActionEdicao"]);
            Assert.AreEqual("/ProcessoCotacaoMaterial/EditarCadastro", _controller.ViewData["ActionEdicao"]);
        }

        [TestMethod]
        public void QuandoEstaConectadoComOPerfilFornecedorViewBagContemActionDeEdicaoEsperada()
        {
            var usuarioConectado = new UsuarioConectado("fornecedor", "Usuário Fornecedor", 2);
            var controller = new ProcessoCotacaoMaterialController(_consultaProcessoCotacaoMaterialMock.Object, _consultaIvaMock.Object);
            CommonMocks.MockControllerUrl(controller);
            controller.Index();
            Assert.IsNotNull(controller.ViewData["ActionEdicao"]);
            Assert.AreEqual("/Cotacao/EditarCadastro", controller.ViewData["ActionEdicao"]);
        }


    }
}

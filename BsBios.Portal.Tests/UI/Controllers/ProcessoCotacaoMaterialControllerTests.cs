using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProcessoCotacaoMaterialControllerTests
    {
        private readonly Mock<IConsultaProcessoDeCotacaoDeMaterial> _consultaProcessoCotacaoMaterialMock;
        private readonly Mock<IConsultaStatusProcessoCotacao> _consultaStatusProcessoCotacao;
        private readonly ProcessoDeCotacaoDeMaterialController _controller;


        public ProcessoCotacaoMaterialControllerTests()
        {
            _consultaProcessoCotacaoMaterialMock = new Mock<IConsultaProcessoDeCotacaoDeMaterial>(MockBehavior.Strict);
            _consultaProcessoCotacaoMaterialMock.Setup(x => x.ConsultaProcesso(It.IsAny<int>()))
                                               .Returns(new ProcessoCotacaoMaterialCadastroVm());
            _consultaStatusProcessoCotacao = new Mock<IConsultaStatusProcessoCotacao>(MockBehavior.Strict);
            _consultaStatusProcessoCotacao.Setup(x => x.Listar()).Returns(new List<StatusProcessoCotacaoVm>());

            _controller = new ProcessoDeCotacaoDeMaterialController(_consultaProcessoCotacaoMaterialMock.Object, _consultaStatusProcessoCotacao.Object);
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
            Assert.AreEqual("_ProcessoCotacaoIndex", viewResult.ViewName);
        }

        [TestMethod]
        public void ActionIndexRetornaListaDeStatusDoProcessoDeCotacaoNaViewBag()
        {
            _controller.Index();
            Assert.IsNotNull(_controller.ViewBag.StatusProcessoCotacao);
        }
        [TestMethod]
        public void QuandoEstaConectadoComOPerfilCompradorViewBagContemActionDeEdicaoEsperada()
        {
            //_usuarioConectado = new UsuarioConectado("comprador", "Usuário Comprador", 1);
            _controller.Index();
            Assert.IsNotNull(_controller.ViewData["ActionEdicao"]);
            Assert.AreEqual("/ProcessoDeCotacaoDeMaterial/EditarCadastro", _controller.ViewData["ActionEdicao"]);
        }

        [TestMethod]
        public void QuandoEstaConectadoComOPerfilFornecedorViewBagContemActionDeEdicaoEsperada()
        {
            ObjectFactory.Configure(x => x.For<UsuarioConectado>()
                .HybridHttpOrThreadLocalScoped()
                .Use(new UsuarioConectado("fornecedor", "Usuário Fornecedor", new List<Enumeradores.Perfil>{Enumeradores.Perfil.Fornecedor})));

            var controller = new ProcessoDeCotacaoDeMaterialController(_consultaProcessoCotacaoMaterialMock.Object, _consultaStatusProcessoCotacao.Object);
            CommonMocks.MockControllerUrl(controller);
            controller.Index();
            Assert.IsNotNull(controller.ViewData["ActionEdicao"]);
            Assert.AreEqual("/CotacaoMaterial/EditarCadastro", controller.ViewData["ActionEdicao"]);

            ObjectFactory.Configure(x => x.For<UsuarioConectado>()
                .HybridHttpOrThreadLocalScoped()
                .Use(new UsuarioConectado("teste", "Usuário de Teste", new List<Enumeradores.Perfil>{Enumeradores.Perfil.CompradorSuprimentos})));
        }


    }
}

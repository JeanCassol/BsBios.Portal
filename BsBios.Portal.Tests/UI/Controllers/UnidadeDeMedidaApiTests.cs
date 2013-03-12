using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class UnidadeDeMedidaApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeUnidadesDeMedidaComSucessoDeveRetornarStatusOk()
        {
            var cadastroUnidadeMedidaMock = new Mock<ICadastroUnidadeDeMedida>(MockBehavior.Strict);
            cadastroUnidadeMedidaMock.Setup(x => x.AtualizarUnidadesDeMedida(It.IsAny<IList<UnidadeDeMedidaCadastroVm>>()));
            var unidadeMedidaApiController = new UnidadeMedidaApiController(cadastroUnidadeMedidaMock.Object);
            var unidadeDeMedidaCadastroVm = new UnidadeDeMedidaCadastroVm()
            {
                CodigoInterno = "I01",
                CodigoExterno = "E01",
                Descricao = "UNIDADE 01"
            };
            unidadeMedidaApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/UnidadeMedidaApi/PostMultiplo");
            unidadeMedidaApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = unidadeMedidaApiController.PostMultiplo(new ListaUnidadesDeMedida() { unidadeDeMedidaCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("200", apiResponseMessage.Retorno.Codigo);
            cadastroUnidadeMedidaMock.Verify(x => x.AtualizarUnidadesDeMedida(It.IsAny<IList<UnidadeDeMedidaCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeIvaDeveRetornarStatusDeErro()
        {
            var cadastroUnidadeMedidaMock = new Mock<ICadastroUnidadeDeMedida>(MockBehavior.Strict);
            cadastroUnidadeMedidaMock.Setup(x => x.AtualizarUnidadesDeMedida(It.IsAny<IList<UnidadeDeMedidaCadastroVm>>()))
                .Throws(new ExcecaoDeTeste("Ocorreu um erro ao atualizar as unidades de Medida"));
            var unidadeMedidaApiController = new UnidadeMedidaApiController(cadastroUnidadeMedidaMock.Object);
            var unidadeDeMedidaCadastroVm = new UnidadeDeMedidaCadastroVm()
            {
                CodigoInterno = "I01",
                CodigoExterno = "E01",
                Descricao = "UNIDADE 01"
            };
            unidadeMedidaApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/UnidadeMedidaApi/PostMultiplo");
            unidadeMedidaApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = unidadeMedidaApiController.PostMultiplo(new ListaUnidadesDeMedida() { unidadeDeMedidaCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroUnidadeMedidaMock.Verify(x => x.AtualizarUnidadesDeMedida(It.IsAny<IList<UnidadeDeMedidaCadastroVm>>()), Times.Once());
        }
    }
}

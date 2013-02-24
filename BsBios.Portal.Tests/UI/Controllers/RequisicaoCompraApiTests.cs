using System;
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
    public class RequisicaoCompraApiTests
    {
        private static RequisicaoDeCompraVm _requisicaoDeCompraVm;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            _requisicaoDeCompraVm = new RequisicaoDeCompraVm()
            {
                NumeroRequisicao = "REQ001",
                NumeroItem = "0001",
                Centro = "C001",
                Criador = "criador",
                DataDeSolicitacao = DateTime.Today.AddDays(-2).ToShortDateString(),
                DataDeLiberacao = DateTime.Today.AddDays(-1).ToShortDateString(),
                DataDeRemessa = DateTime.Today.ToShortDateString(),
                Descricao = "Requisição de compra enviada pelo SAP",
                FornecedorPretendido = "FORNEC0001",
                Material = "PROD0001",
                Quantidade = 100,
                Requisitante = "requisitante",
                UnidadeMedida = "UND"
            }; 
            
        }

        [TestMethod]
        public void QuandoAdicionarUmaNovaRequisicaoComSucessoDeveRetornarStatusOk()
        {
            var cadastroRequisicaoCompraMock = new Mock<ICadastroRequisicaoCompra>(MockBehavior.Strict);
            cadastroRequisicaoCompraMock.Setup(x => x.NovaRequisicao(It.IsAny<RequisicaoDeCompraVm>()));

            var requisicaoCompraApiController = new RequisicaoCompraApiController(cadastroRequisicaoCompraMock.Object)
                {
                    Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/RequisicaoCompraApi/NovaRequisicao")
                };
            requisicaoCompraApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = requisicaoCompraApiController.NovaRequisicao(_requisicaoDeCompraVm);
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("200", apiResponseMessage.Retorno.Codigo);
            cadastroRequisicaoCompraMock.Verify(x => x.NovaRequisicao(It.IsAny<RequisicaoDeCompraVm>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAdicionarUmaNovaRequisicaoDeveRetornarStatusDeErro()
        {
            var cadastroRequisicaoCompraMock = new Mock<ICadastroRequisicaoCompra>(MockBehavior.Strict);
            cadastroRequisicaoCompraMock.Setup(x => x.NovaRequisicao(It.IsAny<RequisicaoDeCompraVm>()))
                .Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar a requisição"));

            var requisicaoCompraApiController = new RequisicaoCompraApiController(cadastroRequisicaoCompraMock.Object)
                {
                    Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/RequisicaoCompraApi/NovaRequisicao")
                };
            requisicaoCompraApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = requisicaoCompraApiController.NovaRequisicao(_requisicaoDeCompraVm);
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroRequisicaoCompraMock.Verify(x => x.NovaRequisicao(It.IsAny<RequisicaoDeCompraVm>()), Times.Once());
        }
    }
}

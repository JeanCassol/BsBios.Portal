using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class CondicaoPagamentoApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeCondicoesDePagamentoComSucessoDeveRetornarStatusOk()
        {
            var cadastroCondicaoPagametoMock = new Mock<ICadastroCondicaoPagamento>(MockBehavior.Strict);
            cadastroCondicaoPagametoMock.Setup(x => x.AtualizarCondicoesDePagamento(It.IsAny<IList<CondicaoDePagamentoCadastroVm>>()));
            var condicaoPagamentoApiController = new CondicaoPagamentoApiController(cadastroCondicaoPagametoMock.Object);
            var condicaoPagamentoCadastroVm = new CondicaoDePagamentoCadastroVm()
            {
                Codigo = "C001",
                Descricao = "CONDICAO 001"
            };
            condicaoPagamentoApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/CondicaoPagamentoApi/PostMultiplo");
            condicaoPagamentoApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = condicaoPagamentoApiController.PostMultiplo(new ListaCondicaoPagamento() { condicaoPagamentoCadastroVm });

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroCondicaoPagametoMock.Verify(x => x.AtualizarCondicoesDePagamento(It.IsAny<IList<CondicaoDePagamentoCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeCondicoesDePagamentoDeveRetornarStatusDeErro()
        {
            var cadastroCondicaoPagamentoMock = new Mock<ICadastroCondicaoPagamento>(MockBehavior.Strict);
            cadastroCondicaoPagamentoMock.Setup(x => x.AtualizarCondicoesDePagamento(It.IsAny<IList<CondicaoDePagamentoCadastroVm>>()))
                .Throws(new Exception("Ocorreu um erro ao atualizar as condições de pagamento"));
            var condicaoPagamentoApiController = new CondicaoPagamentoApiController(cadastroCondicaoPagamentoMock.Object);
            var condicaoPagamentoCadastroVm = new CondicaoDePagamentoCadastroVm()
            {
                Codigo = "C001",
                Descricao = "CONDICAO 001"
            };
            condicaoPagamentoApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/CondicaoPagamentoApi/PostMultiplo");
            condicaoPagamentoApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = condicaoPagamentoApiController.PostMultiplo(new ListaCondicaoPagamento(){ condicaoPagamentoCadastroVm }) ;
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroCondicaoPagamentoMock.Verify(x => x.AtualizarCondicoesDePagamento(It.IsAny<IList<CondicaoDePagamentoCadastroVm>>()), Times.Once());
        }
    }
}

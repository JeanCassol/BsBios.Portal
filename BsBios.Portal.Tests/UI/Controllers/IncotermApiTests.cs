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
    public class IncotermApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeIncotermsComSucessoDeveRetornarStatusOk()
        {
            var cadastroIncotermMock = new Mock<ICadastroIncoterm>(MockBehavior.Strict);
            cadastroIncotermMock.Setup(x => x.AtualizarIncoterms(It.IsAny<IList<IncotermCadastroVm>>()));
            var incotermApiController = new IncotermApiController(cadastroIncotermMock.Object);
            var incotermCadastroVm = new IncotermCadastroVm()
            {
                Codigo = "001",
                Descricao = "INCOTERM 001"
            };
            incotermApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/IncotermApi/PostMultiplo");
            incotermApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = incotermApiController.PostMultiplo(new ListaIncoterm(){ incotermCadastroVm });

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroIncotermMock.Verify(x => x.AtualizarIncoterms(It.IsAny<IList<IncotermCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeIncotermsDeveRetornarStatusDeErro()
        {
            var cadastroIncotermMock = new Mock<ICadastroIncoterm>(MockBehavior.Strict);
            cadastroIncotermMock.Setup(x => x.AtualizarIncoterms(It.IsAny<IList<IncotermCadastroVm>>()))
                .Throws(new Exception("Ocorreu um erro ao atualizar os Ivas"));

            var incotermApiController = new IncotermApiController(cadastroIncotermMock.Object);
            var incotermCadastroVm = new IncotermCadastroVm()
            {
                Codigo = "001",
                Descricao = "INCOTERM 001"
            };
            incotermApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/IncotermApi/PostMultiplo");
            incotermApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = incotermApiController.PostMultiplo(new ListaIncoterm() { incotermCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroIncotermMock.Verify(x => x.AtualizarIncoterms(It.IsAny<IList<IncotermCadastroVm>>()), Times.Once());
        }
    }
}

using System;
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
    public class ItinerarioApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeItinerariosComSucessoDeveRetornarStatusOk()
        {
            var cadastroItinerarioMock = new Mock<ICadastroItinerario>(MockBehavior.Strict);
            cadastroItinerarioMock.Setup(x => x.AtualizarItinerarios(It.IsAny<IList<ItinerarioCadastroVm>>()));
            var itinerarioApiController = new ItinerarioApiController(cadastroItinerarioMock.Object);
            var itinerarioCadastroVm = new ItinerarioCadastroVm()
            {
                Codigo = "01",
                Descricao = "ITINERARIO 01"
            };
            itinerarioApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ItinerarioApi/PostMultiplo");
            itinerarioApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = itinerarioApiController.PostMultiplo(new ListaItinerarios() { itinerarioCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("200", apiResponseMessage.Retorno.Codigo);
            cadastroItinerarioMock.Verify(x => x.AtualizarItinerarios(It.IsAny<IList<ItinerarioCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeItinerariosDeveRetornarStatusDeErro()
        {
            var cadastroItinerarioMock = new Mock<ICadastroItinerario>(MockBehavior.Strict);
            cadastroItinerarioMock.Setup(x => x.AtualizarItinerarios(It.IsAny<IList<ItinerarioCadastroVm>>()))
                .Throws(new ExcecaoDeTeste("ocorreu um erro ao atualizar os itinerários"));
            var itinerarioApiController = new ItinerarioApiController(cadastroItinerarioMock.Object);
            var itinerarioCadastroVm = new ItinerarioCadastroVm()
            {
                Codigo = "01",
                Descricao = "ITINERARIO 01"
            };
            itinerarioApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ItinerarioApi/PostMultiplo");
            itinerarioApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = itinerarioApiController.PostMultiplo(new ListaItinerarios() { itinerarioCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroItinerarioMock.Verify(x => x.AtualizarItinerarios(It.IsAny<IList<ItinerarioCadastroVm>>()), Times.Once());
        }
    }
}

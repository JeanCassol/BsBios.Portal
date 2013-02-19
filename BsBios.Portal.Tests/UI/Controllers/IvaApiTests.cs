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
    public class IvaApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeIvasComSucessoDeveRetornarStatusOk()
        {
            var cadastroIvaMock = new Mock<ICadastroIva>(MockBehavior.Strict);
            cadastroIvaMock.Setup(x => x.AtualizarIvas(It.IsAny<IList<IvaCadastroVm>>()));
            var ivaApiController = new IvaApiController(cadastroIvaMock.Object);
            var ivaCadastroVm = new IvaCadastroVm()
            {
                Codigo = "01",
                Descricao = "IVA 01"
            };
            ivaApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/IvaApi/PostMultiplo");
            ivaApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = ivaApiController.PostMultiplo(new ListaIva(){ ivaCadastroVm });

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroIvaMock.Verify(x => x.AtualizarIvas(It.IsAny<IList<IvaCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeIvaDeveRetornarStatusDeErro()
        {
            var cadastroIvaMock = new Mock<ICadastroIva>(MockBehavior.Strict);
            cadastroIvaMock.Setup(x => x.AtualizarIvas(It.IsAny<IList<IvaCadastroVm>>()))
                .Throws(new Exception("Ocorreu um erro ao atualizar os Ivas"));
            var ivaApiController = new IvaApiController(cadastroIvaMock.Object);
            var ivaCadastroVm = new IvaCadastroVm()
            {
                Codigo = "FORNEC0001",
                Descricao = "FORNECEDOR 0001"
            };
            ivaApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/IvaApi/PostMultiplo");
            ivaApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = ivaApiController.PostMultiplo(new ListaIva() { ivaCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroIvaMock.Verify(x => x.AtualizarIvas(It.IsAny<IList<IvaCadastroVm>>()), Times.Once());
        }
    }
}

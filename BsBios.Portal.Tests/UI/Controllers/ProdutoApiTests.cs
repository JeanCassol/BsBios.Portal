using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProdutoApiTests
    {
        [TestMethod]
        public void QuandoCadastroUmProdutoComSucessoDeveRetornarStatusOk()
        {
            var cadastroProdutoMock = new Mock<ICadastroProduto>(MockBehavior.Strict);
            cadastroProdutoMock.Setup(x => x.Novo(It.IsAny<ProdutoCadastroVm>()));
            var produtoApiController = new ProdutoApiController(cadastroProdutoMock.Object);
            var produtoCadastroVm = new ProdutoCadastroVm()
                {
                    CodigoSap = "SAP 0001",
                    Descricao = "PRODUTO 0001"
                };
            produtoApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ProdutoApi/Post");

            var resposta = produtoApiController.Post(produtoCadastroVm);
            
            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroProdutoMock.Verify(x => x.Novo(It.IsAny<ProdutoCadastroVm>()),Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerAlgumErroDeveRetornarStatusDeErroComMensagem()
        {
            var cadastroProdutoMock = new Mock<ICadastroProduto>(MockBehavior.Strict);
            cadastroProdutoMock.Setup(x => x.Novo(It.IsAny<ProdutoCadastroVm>())).Throws(new Exception("Ocorreu um erro ao cadastrar o Produto"));
            var produtoApiController = new ProdutoApiController(cadastroProdutoMock.Object);
            var produtoCadastroVm = new ProdutoCadastroVm()
            {
                CodigoSap = "SAP 0001",
                Descricao = "PRODUTO 0001"
            };
            produtoApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ProdutoApi/Post");
            produtoApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = produtoApiController.Post(produtoCadastroVm);

            Assert.AreEqual(HttpStatusCode.InternalServerError, resposta.StatusCode);
            //Assert.AreEqual("Ocorreu um erro ao cadastrar o Produto", resposta.RequestMessage.Content);

            cadastroProdutoMock.Verify(x => x.Novo(It.IsAny<ProdutoCadastroVm>()), Times.Once());
        }

        [TestMethod]
        public void QuandoAtualizarUmaListaDeProdutosComSucessoDeveRetornarStatusOk()
        {
            var cadastroProdutoMock = new Mock<ICadastroProduto>(MockBehavior.Strict);
            cadastroProdutoMock.Setup(x => x.AtualizarProdutos(It.IsAny<IList<ProdutoCadastroVm>>()));
            var produtoApiController = new ProdutoApiController(cadastroProdutoMock.Object);
            var produtoCadastroVm = new ProdutoCadastroVm()
            {
                CodigoSap = "SAP 0001",
                Descricao = "PRODUTO 0001"
            };
            produtoApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ProdutoApi/PostMultiplo");
            produtoApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = produtoApiController.PostMultiplo(new ListaProdutos(){produtoCadastroVm});

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroProdutoMock.Verify(x => x.AtualizarProdutos(It.IsAny<IList<ProdutoCadastroVm>>()), Times.Once());            
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeProdutosDeveRetornarStatusDeErro()
        {
            var cadastroProdutoMock = new Mock<ICadastroProduto>(MockBehavior.Strict);
            cadastroProdutoMock.Setup(x => x.AtualizarProdutos(It.IsAny<IList<ProdutoCadastroVm>>()))
                .Throws(new Exception("Ocorreu um erro ao atualizar os produtos"));
            var produtoApiController = new ProdutoApiController(cadastroProdutoMock.Object);
            var produtoCadastroVm = new ProdutoCadastroVm()
            {
                CodigoSap = "SAP 0001",
                Descricao = "PRODUTO 0001"
            };
            produtoApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ProdutoApi/PostMultiplo");
            produtoApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = produtoApiController.PostMultiplo(new ListaProdutos() { produtoCadastroVm });
            var apiResponseMessage =  (ApiResponseMessage) ((ObjectContent) (resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroProdutoMock.Verify(x => x.AtualizarProdutos(It.IsAny<IList<ProdutoCadastroVm>>()), Times.Once());
        }
    }
}

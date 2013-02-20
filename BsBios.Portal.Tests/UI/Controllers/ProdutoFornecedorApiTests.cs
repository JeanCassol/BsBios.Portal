using System;
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
    public class ProdutoFornecedorApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeFornecedoresDeProdutosComSucessoDeveRetornarStatusOk()
        {
            var cadastroProdutoFornecedorMock = new Mock<ICadastroProdutoFornecedor>(MockBehavior.Strict);
            cadastroProdutoFornecedorMock.Setup(x => x.AtualizarFornecedoresDoProduto(It.IsAny<string>(),It.IsAny<string[]>()));
            var produtoFornecedorApiController = new ProdutoFornecedorApiController(cadastroProdutoFornecedorMock.Object);
            var produtoFornecedorCadastroVm = new ProdutoFornecedorCadastroVm()
            {
                CodigoProduto = "PROD0001",
                CodigoFornecedor = "FORNEC0001",
            };
            produtoFornecedorApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ProdutoFornecedorApi/AtualizarFonecedoresDoProduto");
            produtoFornecedorApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = produtoFornecedorApiController.AtualizaFornecedoresDoProduto(new ListaProdutoFornecedor() { produtoFornecedorCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;
            Assert.AreEqual("200", apiResponseMessage.Retorno.Codigo);
            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroProdutoFornecedorMock.Verify(x => x.AtualizarFornecedoresDoProduto(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeProdutosDeveRetornarStatusDeErro()
        {
            var cadastroProdutoFornecedorMock = new Mock<ICadastroProdutoFornecedor>(MockBehavior.Strict);
            cadastroProdutoFornecedorMock.Setup(x => x.AtualizarFornecedoresDoProduto(It.IsAny<string>(), It.IsAny<string[]>()))
                .Throws(new Exception("Ocorreu um erro") );
            var produtoFornecedorApiController = new ProdutoFornecedorApiController(cadastroProdutoFornecedorMock.Object);
            var produtoFornecedorCadastroVm = new ProdutoFornecedorCadastroVm()
            {
                CodigoProduto = "PROD0001",
                CodigoFornecedor = "FORNEC0001",
            };
            produtoFornecedorApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ProdutoFornecedorApi/AtualizarFonecedoresDoProduto");
            produtoFornecedorApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = produtoFornecedorApiController.AtualizaFornecedoresDoProduto(new ListaProdutoFornecedor() { produtoFornecedorCadastroVm });

            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroProdutoFornecedorMock.Verify(x => x.AtualizarFornecedoresDoProduto(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once());
        }

        [TestMethod]
        public void QuandoAtualizaUmaListaComMaisDeUmProdutoDeveRetornarOkParaProdutosAtualizadosComSucessoEErroParaProdutosOndeOcorreuErro()
        {
            var cadastroProdutoFornecedorMock = new Mock<ICadastroProdutoFornecedor>(MockBehavior.Strict);
            cadastroProdutoFornecedorMock
                .Setup(x => x.AtualizarFornecedoresDoProduto(It.IsAny<string>(), It.IsAny<string[]>()))
                .Callback((string codigoProduto, string[] codigoDosFornecedores) =>
                    {
                        if (codigoProduto == "PROD0002")
                        {
                            throw new Exception("Ocorreu erro ao atualizar o produto " + codigoProduto);
                        }
                    });

            var produtoFornecedorApiController = new ProdutoFornecedorApiController(cadastroProdutoFornecedorMock.Object);
            var produtoFornecedorCadastroVm = new ProdutoFornecedorCadastroVm()
            {
                CodigoProduto = "PROD0001",
                CodigoFornecedor = "FORNEC0001",
            };
            produtoFornecedorApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/ProdutoFornecedorApi/AtualizarFonecedoresDoProduto");
            produtoFornecedorApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = produtoFornecedorApiController.AtualizaFornecedoresDoProduto(new ListaProdutoFornecedor()
                {
                    produtoFornecedorCadastroVm,
                    new ProdutoFornecedorCadastroVm()
                        {
                            CodigoProduto = "PROD0002",
                            CodigoFornecedor = "FORNEC0002"
                        }
                });

            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            Assert.IsTrue(apiResponseMessage.Retorno.Texto.Contains("Produto: PROD0001 - 1 fornecedores atualizados;"));
            Assert.IsTrue(apiResponseMessage.Retorno.Texto.Contains("Produto: PROD0002 - erro ao atualizar fornecedores: Ocorreu erro ao atualizar o produto PROD0002;"));
            cadastroProdutoFornecedorMock.Verify(x => x.AtualizarFornecedoresDoProduto(It.IsAny<string>(), It.IsAny<string[]>()), Times.Exactly(2));
            
        }
    }
}

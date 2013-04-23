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
    public class FornecedorApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeFornecedoresComSucessoDeveRetornarStatusOk()
        {
            var cadastroFornecedorMock = new Mock<ICadastroFornecedor>(MockBehavior.Strict);
            cadastroFornecedorMock.Setup(x => x.AtualizarFornecedores(It.IsAny<IList<FornecedorCadastroVm>>()));
            var fornecedorApiController = new FornecedorApiController(cadastroFornecedorMock.Object);
            var fornecedorCadastroVm = new FornecedorCadastroVm()
            {
                Codigo = "FORNEC0001",
                Nome = "PRODUTO 0001",
                Email = "fornecedor@empresa.com.br"

            };
            fornecedorApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/FornecedorApi/PostMultiplo");
            fornecedorApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = fornecedorApiController.PostMultiplo(new ListaFornecedores() { fornecedorCadastroVm });

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroFornecedorMock.Verify(x => x.AtualizarFornecedores(It.IsAny<IList<FornecedorCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeProdutosDeveRetornarStatusDeErro()
        {
            var cadastroFornecedorMock = new Mock<ICadastroFornecedor>(MockBehavior.Strict);
            cadastroFornecedorMock.Setup(x => x.AtualizarFornecedores(It.IsAny<IList<FornecedorCadastroVm>>()))
                .Throws(new Exception("Ocorreu um erro ao atualizar os fornecedores"));
            var produtoApiController = new FornecedorApiController(cadastroFornecedorMock.Object);
            var fornecedorCadastroVm = new FornecedorCadastroVm()
            {
                Codigo = "FORNEC0001",
                Nome = "FORNECEDOR 0001",
                Email = "fornecedor@empresa.com.br"
            };
            produtoApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/FornecedorApi/PostMultiplo");
            produtoApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = produtoApiController.PostMultiplo(new ListaFornecedores() { fornecedorCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroFornecedorMock.Verify(x => x.AtualizarFornecedores(It.IsAny<IList<FornecedorCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoEnviarFornecedorSemNomeDeveProcessarOutrosFornecedores()
        {
            var cadastroFornecedorMock = new Mock<ICadastroFornecedor>(MockBehavior.Strict);
            cadastroFornecedorMock.Setup(x => x.AtualizarFornecedores(It.IsAny<IList<FornecedorCadastroVm>>()));
            var fornecedorApiController = new FornecedorApiController(cadastroFornecedorMock.Object);
            var fornecedorCadastroVmComNome = new FornecedorCadastroVm()
            {
                Codigo = "FORNEC0001",
                Nome = "PRODUTO 0001",
                Email = "fornecedor@empresa.com.br"
            };
            var fornecedorCadastroVmSemNome = new FornecedorCadastroVm()
            {
                Codigo = "FORNEC0002",
                Email = "fornecedor@empresa.com.br"

            };

            fornecedorApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/FornecedorApi/PostMultiplo");
            fornecedorApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = fornecedorApiController.PostMultiplo(new ListaFornecedores() { fornecedorCadastroVmComNome, fornecedorCadastroVmSemNome });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("200", apiResponseMessage.Retorno.Codigo);
            Assert.AreEqual("1 fornecedores atualizados.1 fornecedores não atualizados: FORNEC0002.", apiResponseMessage.Retorno.Texto);
            cadastroFornecedorMock.Verify(x => x.AtualizarFornecedores(It.IsAny<IList<FornecedorCadastroVm>>()), Times.Once());

            
        }
    }
}

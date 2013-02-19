using System;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class CadastroProdutoOperacaoTests
    {
        [TestMethod]
        public void QuandoCrirNovoProdutoDeveRetornarMesmasPropriedades()
        {
            var produtoCadastroVm = new ProdutoCadastroVm()
                {
                    CodigoSap = "PROD0002",
                    Descricao = "PRODUTO 0002",
                    Tipo = "02"
                };

            var cadastroProdutoOperacao = new CadastroProdutoOperacao();
            Produto produtoCriado = cadastroProdutoOperacao.Criar(produtoCadastroVm);

            Assert.IsNotNull(produtoCriado);
            Assert.AreEqual("PROD0002", produtoCriado.Codigo);
            Assert.AreEqual("PRODUTO 0002", produtoCriado.Descricao);
            Assert.AreEqual("02", produtoCriado.Tipo);

        }

        [TestMethod]
        public void QuandoAtualizarProdutoDeveAtualizarAsPropriedades()
        {
            var produto = new Produto("PROD0001", "PRODUTO 0001", "01");
            var cadastroProdutoOperacao = new CadastroProdutoOperacao();
            var produtoCadastroVm = new ProdutoCadastroVm()
            {
                CodigoSap = "PROD0002",
                Descricao = "PRODUTO 0002",
                Tipo = "02"
            };
            cadastroProdutoOperacao.Atualizar(produto,produtoCadastroVm);

            Assert.IsNotNull(produto);
            Assert.AreEqual("PROD0001", produto.Codigo);
            Assert.AreEqual("PRODUTO 0002", produto.Descricao);
            Assert.AreEqual("02", produto.Tipo);


        }
    }
}

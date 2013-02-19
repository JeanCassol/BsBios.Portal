using System;
using BsBios.Portal.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class ProdutoTests
    {
        [TestMethod]
        public void QuandoCrioUmProdutoConsigoAcessarAsPropriedades()
        {
            var produto = new Produto("SAP0001", "Produto de Teste","01");
            Assert.AreEqual("SAP0001", produto.Codigo);
            Assert.AreEqual("Produto de Teste", produto.Descricao);
            Assert.AreEqual("01", produto.Tipo);
        }

        [TestMethod]
        public void QuandoAtualizarDescricaoDeveAcessarNovoValor()
        {
            var produto = new Produto("SAP0001", "Produto de Teste","01");
            produto.Atualizar("Produto de Teste atualizado","02" );
            Assert.AreEqual("Produto de Teste atualizado", produto.Descricao);
            Assert.AreEqual("02",produto.Tipo);
        }
    }
}

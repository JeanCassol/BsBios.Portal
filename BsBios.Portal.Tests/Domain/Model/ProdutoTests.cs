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
            var produto = new Produto("SAP0001", "Produto de Teste");
            Assert.AreEqual("SAP0001", produto.CodigoSap);
            Assert.AreEqual("Produto de Teste", produto.Descricao);
            Assert.AreEqual(0, produto.Id);
        }
    }
}

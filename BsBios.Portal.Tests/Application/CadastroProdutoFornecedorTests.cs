using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Application
{
    [TestClass]
    public class CadastroProdutoFornecedorTests
    {
        [TestMethod]
        public void QuandoAtualizoListaDeFornecedosDoProdutoOcorrePersistencia()
        {
            //_cadastroProduto.AtualizarFornecedoresDoProduto("PROD001", new[] { "FORNEC0001", "FORNEC0002" });
            Assert.Fail();

        }

        [TestMethod]
        public void QuandoAtualizoListaDeFornecedoresComSucessoERealizadoCommit()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void QuandoAtualizoListaDeFornecedoresComErroERealizadoRollBack()
        {
            Assert.Fail();
        }
    }
}

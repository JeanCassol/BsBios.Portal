
using BsBios.Portal.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class FornecedorTests
    {
        [TestMethod]
        public void QuandoCrioUmFornecedorConsigoAcessarAsPropriedades()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001","fornecedor@empresa.com.br");
            Assert.AreEqual("FORNEC0001", fornecedor.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", fornecedor.Nome);
            Assert.AreEqual("fornecedor@empresa.com.br",fornecedor.Email);
        }

        [TestMethod]
        public void QuandoAlterarUmFornecedorConsigoAcessarOsNovosValores()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor@empresa.com.br");
            fornecedor.Atualizar("NOVO FORNECEDOR 0001", "novofornecedor@empresa.com.br");
            Assert.AreEqual("NOVO FORNECEDOR 0001", fornecedor.Nome);
            Assert.AreEqual("novofornecedor@empresa.com.br",fornecedor.Email);
        }
    }
}

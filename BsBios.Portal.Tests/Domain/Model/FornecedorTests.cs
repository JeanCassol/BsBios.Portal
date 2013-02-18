using BsBios.Portal.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class FornecedorTests
    {
        [TestMethod]
        public void QuandoCrioUmFornecedorConsigoAcessarAsPropriedades()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001");
            Assert.AreEqual("FORNEC0001", fornecedor.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", fornecedor.Nome);
        }
    }
}

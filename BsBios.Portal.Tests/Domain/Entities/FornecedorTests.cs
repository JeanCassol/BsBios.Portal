using BsBios.Portal.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class FornecedorTests
    {
        [TestMethod]
        public void QuandoCrioUmFornecedorConsigoAcessarAsPropriedades()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001","fornecedor@empresa.com.br","cnpj","municipio", "uf");
            Assert.AreEqual("FORNEC0001", fornecedor.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", fornecedor.Nome);
            Assert.AreEqual("fornecedor@empresa.com.br",fornecedor.Email);
            Assert.AreEqual("cnpj", fornecedor.Cnpj);
            Assert.AreEqual("municipio", fornecedor.Municipio);
            Assert.AreEqual("uf", fornecedor.Uf);
        }

        [TestMethod]
        public void QuandoAlterarUmFornecedorConsigoAcessarOsNovosValores()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor@empresa.com.br", "cnpj", "municipio", "uf");
            fornecedor.Atualizar("NOVO FORNECEDOR 0001", "novofornecedor@empresa.com.br", "novocnpj", "novo municipio", "nova uf");
            Assert.AreEqual("NOVO FORNECEDOR 0001", fornecedor.Nome);
            Assert.AreEqual("novofornecedor@empresa.com.br",fornecedor.Email);
            Assert.AreEqual("novocnpj", fornecedor.Cnpj);
            Assert.AreEqual("novo municipio", fornecedor.Municipio);
            Assert.AreEqual("nova uf", fornecedor.Uf);
        }
    }
}

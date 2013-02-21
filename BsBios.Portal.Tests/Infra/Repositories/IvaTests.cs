using System.Linq;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class IvaTests: RepositoryTest
    {
        private static IIvas _ivas;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverIvasCadastrados();
            _ivas = ObjectFactory.GetInstance<IIvas>();
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void QuandoPersistoUmIvaComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();
            var iva = new Iva("01", "IVA 01");
            _ivas.Save(iva);
            UnitOfWorkNh.Commit();

            Iva ivaConsultado = _ivas.BuscaPeloCodigoSap("01");
            Assert.IsNotNull(ivaConsultado);
            Assert.AreEqual(iva.Codigo, ivaConsultado.Codigo);
        }

        [TestMethod]
        public void QuandoConsultoUmIvaComCodigoSapInexistenteDeveRetornarNulo()
        {
            var iva = _ivas.BuscaPeloCodigoSap("02");
            Assert.IsNull(iva);
        }

        //[TestMethod]
        //public void Teste()
        //{
        //    var produtos = ObjectFactory.GetInstance<IProdutos>();
        //    Produto produtoConsultado = produtos.BuscaPeloCodigo("PROD0001");

        //    Assert.AreEqual(2, produtoConsultado.Fornecedores.Count);

        //    Assert.IsNotNull(produtoConsultado.Fornecedores);

        //    var fornecedor1 = produtoConsultado.Fornecedores.FirstOrDefault(f => f.Codigo == "FORNEC0001");
        //    Assert.IsNotNull(fornecedor1);
        //}

    }

}

using System;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class FornecedoresTests:RepositoryTest
    {
        private readonly IFornecedores _fornecedores;

        public FornecedoresTests()
        {
            _fornecedores = ObjectFactory.GetInstance<IFornecedores>();
        }

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverFornecedoresCadastrados();
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }
        [TestMethod]
        public void QuandoPersistoUmFornecedorComSucessoConsigoConsultarPosteriormente()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001");

            UnitOfWorkNh.BeginTransaction();
            _fornecedores.Save(fornecedor);
            UnitOfWorkNh.Commit();

            Fornecedor fornecedorConsulta = _fornecedores.BuscaPeloCodigoSap("FORNEC0001");

            Assert.IsNotNull(fornecedorConsulta);
            Assert.AreEqual(fornecedor.Id, fornecedorConsulta.Id);
        }

        [TestMethod]
        public void QuandoConsultoUmFornecedorComCodigoSapInexistenteDeveRetornarNulo()
        {
            var fornecedor = _fornecedores.BuscaPeloCodigoSap("FORNEC0002");
            Assert.IsNull(fornecedor);
        }
    }
}

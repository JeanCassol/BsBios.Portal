using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class IncotermsTests: RepositoryTest
    {
        private static IIncoterms _incoterms;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverIncotermsCadastrados();
            _incoterms = ObjectFactory.GetInstance<IIncoterms>();
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void QuandoPersistoUmIncotermComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();
            var incoterm = new Incoterm("001", "INCOTERM 001");
            _incoterms.Save(incoterm);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            Incoterm incotermConsultado = _incoterms.BuscaPeloCodigo("001").Single();
            Assert.IsNotNull(incotermConsultado);
            Assert.AreEqual("001", incotermConsultado.Codigo);
            Assert.AreEqual("INCOTERM 001", incotermConsultado.Descricao);
        }

    }

}

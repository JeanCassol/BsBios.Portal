using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
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
            var incoterm = DefaultObjects.ObtemIncotermPadrao();
            _incoterms.Save(incoterm);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            Incoterm incotermConsultado = _incoterms.BuscaPeloCodigo(incoterm.Codigo).Single();
            Assert.IsNotNull(incotermConsultado);
            Assert.AreEqual(incoterm.Codigo, incotermConsultado.Codigo);
            Assert.AreEqual(incoterm.Descricao, incotermConsultado.Descricao);
        }

    }

}

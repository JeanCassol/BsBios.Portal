using System.Collections.Generic;
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

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
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
            UnitOfWorkNh.Session.Save(incoterm);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var incoterms = ObjectFactory.GetInstance<IIncoterms>();

            Incoterm incotermConsultado = incoterms.BuscaPeloCodigo(incoterm.Codigo).Single();
            Assert.IsNotNull(incotermConsultado);
            Assert.AreEqual(incoterm.Codigo, incotermConsultado.Codigo);
            Assert.AreEqual(incoterm.Descricao, incotermConsultado.Descricao);
        }

        [TestMethod]
        public void QuandoFiltroPorListaDeCodigoTemQueRetornarIncotermsCorrespondenteAosCodigos()
        {
            UnitOfWorkNh.BeginTransaction();
            Incoterm incoterm1 = DefaultObjects.ObtemIncotermPadrao();
            Incoterm incoterm2 = DefaultObjects.ObtemIncotermPadrao();
            Incoterm incoterm3 = DefaultObjects.ObtemIncotermPadrao();
            UnitOfWorkNh.Session.Save(incoterm1);
            UnitOfWorkNh.Session.Save(incoterm2);
            UnitOfWorkNh.Session.Save(incoterm3);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var incoterms = ObjectFactory.GetInstance<IIncoterms>();

            IList<Incoterm> incotermsConsultados = incoterms.FiltraPorListaDeCodigos(new[] {incoterm1.Codigo, incoterm2.Codigo}).List();

            Assert.AreEqual(2, incotermsConsultados.Count);
            Assert.AreEqual(1, incotermsConsultados.Count(x => x.Codigo == incoterm1.Codigo));
            Assert.AreEqual(1, incotermsConsultados.Count(x => x.Codigo == incoterm2.Codigo));

        }

    }

}

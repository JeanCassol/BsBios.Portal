using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class ItinerariosTests: RepositoryTest
    {
        private static IItinerarios _itinerarios;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            _itinerarios = ObjectFactory.GetInstance<IItinerarios>();
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void QuandoPersistoUmItinerarioComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            _itinerarios.Save(itinerario);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            Itinerario itinerarioConsultado = _itinerarios.BuscaPeloCodigo(itinerario.Codigo).Single();
            Assert.IsNotNull(itinerarioConsultado);
            Assert.AreEqual(itinerario.Codigo, itinerarioConsultado.Codigo);
            Assert.AreEqual(itinerario.Descricao, itinerarioConsultado.Descricao);
        }

        [TestMethod]
        public void ConsigoCadastrarUmItinerarioComCaracteresEspeciaisNaDescricao()
        {
            UnitOfWorkNh.BeginTransaction();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            itinerario.AtualizaDescricao("RS Rio Grande -> AL Olho D Água Das Flor");
            _itinerarios.Save(itinerario);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            Itinerario itinerarioConsultado = _itinerarios.BuscaPeloCodigo(itinerario.Codigo).Single();
            Assert.IsNotNull(itinerarioConsultado);
            Assert.AreEqual(itinerario.Codigo, itinerarioConsultado.Codigo);
            Assert.AreEqual(itinerario.Descricao, itinerarioConsultado.Descricao);
            
        }

    }

}

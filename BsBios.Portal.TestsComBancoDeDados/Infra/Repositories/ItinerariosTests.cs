using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using System.Linq;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class ItinerariosTests: RepositoryTest
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
        public void QuandoPersistoUmItinerarioComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            UnitOfWorkNh.Session.Save(itinerario);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();
            
            var itinerarios = ObjectFactory.GetInstance<IItinerarios>();
            Itinerario itinerarioConsultado = itinerarios.BuscaPeloCodigo(itinerario.Codigo).Single();
            Assert.IsNotNull(itinerarioConsultado);
            Assert.AreEqual(itinerario.Codigo, itinerarioConsultado.Codigo);
            Assert.AreEqual(itinerario.Descricao, itinerarioConsultado.Descricao);
        }

        [TestMethod]
        public void ConsigoCadastrarUmItinerarioComAcentuacaoEspeciaisNaDescricao()
        {
            UnitOfWorkNh.BeginTransaction();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            itinerario.AtualizaDescricao("RS Rio Grande -> AL Olho D Água Das Flor");
            UnitOfWorkNh.Session.Save(itinerario);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var itinerarios = ObjectFactory.GetInstance<IItinerarios>();
            Itinerario itinerarioConsultado = itinerarios.BuscaPeloCodigo(itinerario.Codigo).Single();
            Assert.IsNotNull(itinerarioConsultado);
            Assert.AreEqual(itinerario.Codigo, itinerarioConsultado.Codigo);
            Assert.AreEqual(itinerario.Descricao, itinerarioConsultado.Descricao);
            
        }

        [TestMethod]
        public void QuandoFiltroPorListaDeCodigoTemQueRetornarItinerariosCorrespondenteAosCodigos()
        {
            UnitOfWorkNh.BeginTransaction();
            Itinerario itinerario1 = DefaultObjects.ObtemItinerarioPadrao();
            Itinerario itinerario2 = DefaultObjects.ObtemItinerarioPadrao();
            Itinerario itinerario3 = DefaultObjects.ObtemItinerarioPadrao();
            UnitOfWorkNh.Session.Save(itinerario1);
            UnitOfWorkNh.Session.Save(itinerario2);
            UnitOfWorkNh.Session.Save(itinerario3);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var itinerarios = ObjectFactory.GetInstance<IItinerarios>();
            IList<Itinerario> itinerariosConsultados = itinerarios.FiltraPorListaDeCodigos(new[] { itinerario1.Codigo, itinerario2.Codigo }).List();

            Assert.AreEqual(2, itinerariosConsultados.Count);
            Assert.AreEqual(1, itinerariosConsultados.Count(x => x.Codigo == itinerario1.Codigo));
            Assert.AreEqual(1, itinerariosConsultados.Count(x => x.Codigo == itinerario2.Codigo));

        }



    }

}

using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using System.Linq;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class UnidadesDeMedidaTests: RepositoryTest
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
            UnidadeDeMedida unidadeDeMedida = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            UnitOfWorkNh.Session.Save(unidadeDeMedida);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var unidadesDeMedida = ObjectFactory.GetInstance<IUnidadesDeMedida>();

            UnidadeDeMedida unidadeDeMedidaConsultada = unidadesDeMedida.BuscaPeloCodigoInterno(unidadeDeMedida.CodigoInterno).Single();
            Assert.IsNotNull(unidadeDeMedidaConsultada);
            Assert.AreEqual(unidadeDeMedida.CodigoInterno, unidadeDeMedidaConsultada.CodigoInterno);
            Assert.AreEqual(unidadeDeMedida.CodigoExterno, unidadeDeMedidaConsultada.CodigoExterno);
            Assert.AreEqual(unidadeDeMedida.Descricao, unidadeDeMedidaConsultada.Descricao);
        }

        [TestMethod]
        public void QuandoFiltroPorListaDeCodigoInternoTemQueRetornarUnidadesDeMedidaCorrespondentesAosCodigos()
        {
            UnitOfWorkNh.BeginTransaction();
            UnidadeDeMedida unidadeDeMedida1 = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            UnidadeDeMedida unidadeDeMedida2 = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            UnidadeDeMedida unidadeDeMedida3 = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            UnitOfWorkNh.Session.Save(unidadeDeMedida1);
            UnitOfWorkNh.Session.Save(unidadeDeMedida2);
            UnitOfWorkNh.Session.Save(unidadeDeMedida3);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var unidadesDeMedida = ObjectFactory.GetInstance<IUnidadesDeMedida>();

            IList<UnidadeDeMedida> unidadesDeMedidaConsultadas = unidadesDeMedida.FiltraPorListaDeCodigosInternos(new[] { unidadeDeMedida1.CodigoInterno, unidadeDeMedida2.CodigoInterno }).List();

            Assert.AreEqual(2, unidadesDeMedidaConsultadas.Count);
            Assert.AreEqual(1, unidadesDeMedidaConsultadas.Count(x => x.CodigoInterno == unidadeDeMedida1.CodigoInterno));
            Assert.AreEqual(1, unidadesDeMedidaConsultadas.Count(x => x.CodigoInterno == unidadeDeMedida2.CodigoInterno));

        }

    }

}

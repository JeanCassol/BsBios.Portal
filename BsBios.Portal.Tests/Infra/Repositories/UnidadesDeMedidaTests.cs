using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class UnidadesDeMedidaTests: RepositoryTest
    {
        private static IUnidadesDeMedida _unidadesDeMedida;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            _unidadesDeMedida = ObjectFactory.GetInstance<IUnidadesDeMedida>();
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
            _unidadesDeMedida.Save(unidadeDeMedida);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            UnidadeDeMedida unidadeDeMedidaConsultada = _unidadesDeMedida.BuscaPeloCodigoInterno(unidadeDeMedida.CodigoInterno).Single();
            Assert.IsNotNull(unidadeDeMedidaConsultada);
            Assert.AreEqual(unidadeDeMedida.CodigoInterno, unidadeDeMedidaConsultada.CodigoInterno);
            Assert.AreEqual(unidadeDeMedida.CodigoExterno, unidadeDeMedidaConsultada.CodigoExterno);
            Assert.AreEqual(unidadeDeMedida.Descricao, unidadeDeMedidaConsultada.Descricao);
        }

    }

}

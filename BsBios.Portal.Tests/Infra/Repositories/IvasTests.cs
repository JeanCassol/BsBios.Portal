using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using System.Linq;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class IvasTests: RepositoryTest
    {
        private static IIvas _ivas;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
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

            Iva ivaConsultado = _ivas.BuscaPeloCodigo("01");
            Assert.IsNotNull(ivaConsultado);
            Assert.AreEqual(iva.Codigo, ivaConsultado.Codigo);
        }

        [TestMethod]
        public void QuandoConsultoUmIvaComCodigoSapInexistenteDeveRetornarNulo()
        {
            var iva = _ivas.BuscaPeloCodigo("02");
            Assert.IsNull(iva);
        }

        [TestMethod]
        public void QuandoFiltraPorListaDeCodigosDeIvaRetornaListaDeIvasCorrespondenteAosCodigos()
        {
            Iva iva1 = DefaultObjects.ObtemIvaPadrao();
            Iva iva2 = DefaultObjects.ObtemIvaPadrao();
            Iva iva3 = DefaultObjects.ObtemIvaPadrao();
            DefaultPersistedObjects.PersistirIva(iva1);
            DefaultPersistedObjects.PersistirIva(iva2);
            DefaultPersistedObjects.PersistirIva(iva3);

            string[] codigosIva = {iva1.Codigo, iva2.Codigo};
            var ivas = ObjectFactory.GetInstance<IIvas>();
            IList<Iva> ivasSelecionados = ivas.BuscaListaPorCodigo(codigosIva).List();
            Assert.AreEqual(2, ivasSelecionados.Count);
            Assert.AreEqual(1, ivasSelecionados.Count(x => x.Codigo == iva1.Codigo));
            Assert.AreEqual(1, ivasSelecionados.Count(x => x.Codigo == iva2.Codigo));

        }




    }

}

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
        public void QuandoPersistoUmIvaComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();
            var iva = DefaultObjects.ObtemIvaPadrao();
            UnitOfWorkNh.Session.Save(iva);
            UnitOfWorkNh.Commit();

            var ivas = ObjectFactory.GetInstance<IIvas>();
            Iva ivaConsultado = ivas.BuscaPeloCodigo(iva.Codigo);
            Assert.IsNotNull(ivaConsultado);
            Assert.AreEqual(iva.Codigo, ivaConsultado.Codigo);
            Assert.AreEqual(iva.Descricao, ivaConsultado.Descricao);
        }

        [TestMethod]
        public void QuandoConsultoUmIvaComCodigoSapInexistenteDeveRetornarNulo()
        {
            var ivas = ObjectFactory.GetInstance<IIvas>();
            var iva = ivas.BuscaPeloCodigo("02");
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

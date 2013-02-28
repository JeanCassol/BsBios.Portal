using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class ProcessosDeCotacaoDeMaterialTests: RepositoryTest
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

        //[TestMethod]
        //public void QueryPersonalizada()
        //{
        //    //var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();
        //    var queryable = UnitOfWorkNh.Session.Query<ProcessoDeCotacao>();
        //    IList<ProdutoCadastroVm> produtos = (from pcm in queryable //queryPcm
        //                 where pcm.Id == 5
        //                 && pcm is ProcessoDeCotacaoDeMaterial
        //                 let processo = (ProcessoDeCotacaoDeMaterial)pcm
        //                 select new ProdutoCadastroVm()
        //                 {
        //                     CodigoSap = processo.RequisicaoDeCompra.Material.Codigo,
        //                     Descricao = processo.RequisicaoDeCompra.Material.Descricao,
        //                     Tipo = processo.RequisicaoDeCompra.Material.Tipo,
        //                 }).ToList();
            
        //    Assert.IsTrue(produtos.Count > 0);

        //}

        [TestMethod]
        public void DepoisDePersistirUmProcessoDeCotacaoDeMaterialConsigoConsultar()
        {
            var processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            DefaultPersistedObjects.PersistirRequisicaoDeCompra(processoDeCotacaoDeMaterial.RequisicaoDeCompra);

            UnitOfWorkNh.BeginTransaction();
            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            processosDeCotacaoDeMaterial.Save(processoDeCotacaoDeMaterial);
            UnitOfWorkNh.Commit();

            UnitOfWorkNh.Session.Clear();

            var processoConsultado = (ProcessoDeCotacaoDeMaterial) processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();

            Assert.IsNotNull(processoConsultado);
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.NaoIniciado, processoConsultado.Status);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id ,processoConsultado.Id);
            Assert.IsNull(processoConsultado.DataLimiteDeRetorno);
            Assert.IsFalse(NHibernateUtil.IsInitialized(processoConsultado.RequisicaoDeCompra));
            Assert.AreEqual(processoDeCotacaoDeMaterial.RequisicaoDeCompra.Id, processoConsultado.RequisicaoDeCompra.Id);
        }

    }
}

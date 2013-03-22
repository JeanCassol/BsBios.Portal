using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaQuotaTests: RepositoryTest
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
        public void QuandoConsultaQuotaEmUmaDataQuePossuiQuotaRetornaVerdadeiro()
        {
            //RemoveQueries.RemoverQuotasCadastradas();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000",DateTime.Today,1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            Assert.IsTrue(consultaQuota.PossuiQuotaNaData(DateTime.Today));
           
        }
        [TestMethod]
        public void QuandoConsultaQuotaEmUmaDataQueNaoPossuiQuotaRetornaFalso()
        {
            //RemoveQueries.RemoverQuotasCadastradas();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000", DateTime.Today, 1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            Assert.IsFalse(consultaQuota.PossuiQuotaNaData(DateTime.Today.AddDays(1)));
            
        }

        [TestMethod]
        public void QuandoConsultaQuotasEmUmaDeterminadaDataRetornaListaDeQuotasDeTodosOsFornecedoresDaquelaData()
        {
            RemoveQueries.RemoverQuotasCadastradas();
            Quota quota = DefaultObjects.ObtemQuota();
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            IList<QuotaConsultarVm> quotas = consultaQuota.QuotasDaData(quota.Data);

            Assert.AreEqual(1, quotas.Count);
        }
    }
}

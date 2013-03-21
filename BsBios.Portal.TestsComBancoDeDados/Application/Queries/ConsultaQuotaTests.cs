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
        private static IConsultaQuota _consultaQuota;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            _consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
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
            var quota = new Quota(Enumeradores.FluxoDeCarga.Carregamento, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000",DateTime.Today,1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            Assert.IsTrue(_consultaQuota.PossuiQuotaNaData(DateTime.Today));
           
        }
        [TestMethod]
        public void QuandoConsultaQuotaEmUmaDataQueNaoPossuiQuotaRetornaFalso()
        {
            //RemoveQueries.RemoverQuotasCadastradas();
            var quota = new Quota(Enumeradores.FluxoDeCarga.Carregamento, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000", DateTime.Today, 1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            Assert.IsFalse(_consultaQuota.PossuiQuotaNaData(DateTime.Today.AddDays(1)));
            
        }

        [TestMethod]
        public void QuandoConsultaQuotasEmUmaDeterminadaDataRetornaListaDeQuotasDeTodosOsFornecedoresDaquelaData()
        {
            RemoveQueries.RemoverQuotasCadastradas();
            Quota quota = DefaultObjects.ObtemQuota();
            DefaultPersistedObjects.PersistirQuota(quota);

            IList<QuotaConsultarVm> quotas =  _consultaQuota.QuotasDaData(quota.Data);

            Assert.AreEqual(1, quotas.Count);
        }
    }
}

using System;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class QuotasTests: RepositoryTest
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
        public void ConsigoCriarUmaQuotaEConsultarPosteriormente()
        {
            var quota = DefaultObjects.ObtemQuota();
            DefaultPersistedObjects.PersistirQuota(quota);

            var quotas = ObjectFactory.GetInstance<IQuotas>();

            var quotaConsultada = quotas.FiltraPorData(quota.Data)
                                        .FiltraPorFluxo(quota.FluxoDeCarga)
                                        .FiltraPorTransportadora(quota.Transportadora.Codigo).Single();

            Assert.AreEqual(quota.Data, quotaConsultada.Data);
            Assert.AreEqual(quota.Terminal, quotaConsultada.Terminal);
            Assert.AreEqual(quota.FluxoDeCarga, quotaConsultada.FluxoDeCarga);
            Assert.AreEqual(quota.Material, quotaConsultada.Material);
            Assert.AreEqual(quota.Peso, quotaConsultada.Peso);
            Assert.AreEqual(quota.Transportadora.Codigo, quotaConsultada.Transportadora.Codigo);

        }
    }
}

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
            var quota = DefaultObjects.ObtemQuotaDeCarregamento();
            quota.AdicionarAgendamento(DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(100));
            DefaultPersistedObjects.PersistirQuota(quota);

            var quotas = ObjectFactory.GetInstance<IQuotas>();

            var quotaConsultada = quotas.FiltraPorData(quota.Data)
                                        .FiltraPorFluxo(quota.FluxoDeCarga)
                                        .FiltraPorTransportadora(quota.Fornecedor.Codigo).Single();

            Assert.AreEqual(quota.Data, quotaConsultada.Data);
            Assert.AreEqual(quota.Terminal, quotaConsultada.Terminal);
            Assert.AreEqual(quota.FluxoDeCarga, quotaConsultada.FluxoDeCarga);
            Assert.AreEqual(quota.Material, quotaConsultada.Material);
            Assert.AreEqual(quota.PesoTotal, quotaConsultada.PesoTotal);
            Assert.AreEqual(quota.Fornecedor.Codigo, quotaConsultada.Fornecedor.Codigo);
            Assert.AreEqual(100, quota.PesoAgendado);

        }
    }
}

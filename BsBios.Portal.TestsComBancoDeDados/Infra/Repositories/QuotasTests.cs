using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
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
            //quota.AdicionarAgendamento(DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(quota, 100));
            DefaultPersistedObjects.PersistirQuota(quota);

            var quotas = ObjectFactory.GetInstance<IQuotas>();

            var quotaConsultada = quotas.FiltraPorData(quota.Data)
                                        .FiltraPorFluxo(quota.FluxoDeCarga)
                                        .FiltraPorTransportadora(quota.Fornecedor.Codigo).Single();

            Assert.AreEqual(quota.Id, quotaConsultada.Id);
            Assert.AreEqual(quota.Data, quotaConsultada.Data);
            Assert.AreEqual(quota.CodigoTerminal, quotaConsultada.CodigoTerminal);
            Assert.AreEqual(quota.FluxoDeCarga, quotaConsultada.FluxoDeCarga);
            Assert.AreEqual(quota.Material, quotaConsultada.Material);
            Assert.AreEqual(quota.PesoTotal, quotaConsultada.PesoTotal);
            Assert.AreEqual(quota.Fornecedor.Codigo, quotaConsultada.Fornecedor.Codigo);
            Assert.AreEqual(0, quota.PesoAgendado);

        }

        [TestMethod]
        public void ConsigoPersistirUmAgendamentoParaAQuotaEConsultarPosteriormente()
        {
            var quota = DefaultObjects.ObtemQuotaDeCarregamento();
            var agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(quota, 100);
            quota.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm
                {
                    IdQuota = quota.Id
                });
            DefaultPersistedObjects.PersistirQuota(quota);

            var quotas = ObjectFactory.GetInstance<IQuotas>();

            var quotaConsultada = quotas.BuscaPorId(quota.Id);
            var agendamentoConsultado = (AgendamentoDeCarregamento) quotaConsultada.Agendamentos.First();

            Assert.AreEqual(agendamento.Peso, agendamentoConsultado.Peso);
            Assert.AreEqual(agendamento.Placa, agendamentoConsultado.Placa);
            Assert.AreEqual(agendamento.Realizado, agendamentoConsultado.Realizado);
        }
    }
}

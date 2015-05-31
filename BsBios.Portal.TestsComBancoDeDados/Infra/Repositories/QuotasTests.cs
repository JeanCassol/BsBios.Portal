using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests.DataProvider;
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

            var quotaConsultada = quotas.BuscaPorId(quota.Id);

            Assert.AreEqual(quota.Id, quotaConsultada.Id);
            Assert.AreEqual(quota.Data, quotaConsultada.Data);
            Assert.AreEqual(quota.Terminal.Codigo, quotaConsultada.Terminal.Codigo);
            Assert.AreEqual(quota.FluxoDeCarga, quotaConsultada.FluxoDeCarga);
            Assert.AreEqual(quota.Material.Codigo, quotaConsultada.Material.CastEntity().Codigo);
            Assert.AreEqual(quota.PesoTotal, quotaConsultada.PesoTotal);
            Assert.AreEqual(quota.Fornecedor.Codigo, quotaConsultada.Fornecedor.Codigo);
            Assert.AreEqual(0, quota.PesoAgendado);

        }

        [TestMethod]
        public void ConsigoPersistirUmAgendamentoParaAQuotaEConsultarPosteriormente()
        {
            var quota = DefaultObjects.ObtemQuotaDeCarregamento();
            quota.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm
                {
                    IdQuota = quota.Id,
                    Peso = 100,
                    Placa = "MNO1234",
                    Motorista = "Motorista",
                    Destino = "Destino"
                });
            DefaultPersistedObjects.PersistirQuota(quota);

            var quotas = ObjectFactory.GetInstance<IQuotas>();

            var quotaConsultada = quotas.BuscaPorId(quota.Id);
            var agendamentoConsultado = (AgendamentoDeCarregamento) quotaConsultada.Agendamentos.First();

            Assert.AreEqual(100, agendamentoConsultado.Peso);
            Assert.AreEqual("MNO1234", agendamentoConsultado.Placa);
            Assert.IsFalse(agendamentoConsultado.Realizado);
            Assert.AreEqual(100, agendamentoConsultado.PesoTotal);
            Assert.AreEqual("Motorista", agendamentoConsultado.Motorista);
            Assert.AreEqual("Destino", agendamentoConsultado.Destino);
        }

        [TestMethod]
        public void ConsigoFiltrarQuotaPorTerminal()
        {
            RemoveQueries.RemoverQuotasCadastradas();
            Quota quota1 = DefaultObjects.ObtemQuotaDeDescarregamentoParaTerminalEspecifico("1000");
            Quota quota2 = DefaultObjects.ObtemQuotaDeDescarregamentoParaTerminalEspecifico("2000");
            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota2);

            var quotas = ObjectFactory.GetInstance<IQuotas>();
            IList<Quota> quotasConsultadas = quotas.DoTerminal("1000").List();
            Assert.AreEqual(1, quotasConsultadas.Count());
            var quotaConsultada = quotasConsultadas.First();
            Assert.AreEqual("1000", quotaConsultada.Terminal.Codigo);
        }

        [TestMethod]
        public void ConsigoFiltrarQuotasAPartirDeUmaData()
        {
            RemoveQueries.RemoverQuotasCadastradas();

            var dataDoFiltro = DateTime.Today;
            Quota quotaAntesDoPeriodo = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(dataDoFiltro.AddDays(-1));
            Quota quotaNoPeriodo = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(dataDoFiltro);
            Quota quotaDePoisDoPeriodo = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(dataDoFiltro.AddDays(1));

            DefaultPersistedObjects.PersistirQuota(quotaAntesDoPeriodo);
            DefaultPersistedObjects.PersistirQuota(quotaNoPeriodo);
            DefaultPersistedObjects.PersistirQuota(quotaDePoisDoPeriodo);

            var quotas = ObjectFactory.GetInstance<IQuotas>();

            IList<Quota> quotasConsultadas = quotas.APartirDe(dataDoFiltro).List();
            Assert.AreEqual(2, quotasConsultadas.Count);
            Assert.IsTrue(quotasConsultadas.Any(x => x.Data == dataDoFiltro));
            Assert.IsTrue(quotasConsultadas.Any(x => x.Data == dataDoFiltro.AddDays(1)));
        }

        [TestMethod]
        public void ConsigoFiltrarQuotasAteUmaData()
        {
            RemoveQueries.RemoverQuotasCadastradas();

            var dataDoFiltro = DateTime.Today;
            Quota quotaAntesDoPeriodo = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(dataDoFiltro.AddDays(-1));
            Quota quotaNoPeriodo = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(dataDoFiltro);
            Quota quotaDePoisDoPeriodo = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(dataDoFiltro.AddDays(1));

            DefaultPersistedObjects.PersistirQuota(quotaAntesDoPeriodo);
            DefaultPersistedObjects.PersistirQuota(quotaNoPeriodo);
            DefaultPersistedObjects.PersistirQuota(quotaDePoisDoPeriodo);

            var quotas = ObjectFactory.GetInstance<IQuotas>();

            IList<Quota> quotasConsultadas = quotas.Ate(dataDoFiltro).List();
            Assert.AreEqual(2, quotasConsultadas.Count);
            Assert.IsTrue(quotasConsultadas.Any(x => x.Data == dataDoFiltro));
            Assert.IsTrue(quotasConsultadas.Any(x => x.Data == dataDoFiltro.AddDays(-1)));

        }



        //[TestMethod]
        //public void QuandoRealizadoUmAgendamentoNaoCarregaTodosAgendamentosParaCalcularPesoRealizado()
        //{
        //    var quota = DefaultObjects.ObtemQuotaDeCarregamento();
        //    DefaultPersistedObjects.PersistirQuota(quota);

        //    AgendamentoDeCarregamento agendamento1 = quota.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm
        //    {
        //        IdQuota = quota.Id,
        //        Peso = 100,
        //        Placa = "MNO1234"
        //    });

        //    AgendamentoDeCarregamento agendamento2 = quota.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm
        //    {
        //        IdQuota = quota.Id,
        //        Peso = 50,
        //        Placa = "MNO1235"
        //    });

        //    DefaultPersistedObjects.PersistirQuota(quota);

        //    var quotas = ObjectFactory.GetInstance<IQuotas>();

        //    var quotaConsultada = quotas.BuscaPorId(quota.Id);

        //    quotaConsultada.RealizarAgendamento(agendamento1.Id);
            
        //    Assert.IsFalse(NHibernateUtil.IsInitialized(quotaConsultada.Agendamentos));
        //}
    }
}

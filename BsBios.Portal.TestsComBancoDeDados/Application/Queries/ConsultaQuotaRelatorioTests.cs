using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ConsultaQuotaRelatorioTests: RepositoryTest
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
        public void ConsultaPrevistoRealizadoRetornaOsValoresAgrupadosCorretamente()
        {
            //cria duas quotas, dois fornecedores. Os fornecedores tem agendamentos nas duas quotas. 
            //Apenas uma das quotas de cada fornecedor está realizada.
            RemoveQueries.RemoverQuotasCadastradas();

            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();

            Terminal terminal = DefaultObjects.ObtemTerminalPadrao();

            var quota1 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor1,terminal, DateTime.Today.AddDays(1), 100);
            var quota2 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor1, terminal, DateTime.Today.AddDays(2), 200);
            var quota3 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor2, terminal, DateTime.Today.AddDays(1), 150);
            var quota4 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor2, terminal, DateTime.Today.AddDays(2), 250);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota2);
            DefaultPersistedObjects.PersistirQuota(quota3);
            DefaultPersistedObjects.PersistirQuota(quota4);

            AgendamentoDeCarregamento agendamento1 = quota1.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm {Placa = "IOQ3434", Peso = 30});
            AgendamentoDeCarregamento agendamento2 = quota4.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm { Placa = "IOQ3435", Peso = 250 });

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota4);

            quota1.RealizarAgendamento(agendamento1.Id);
            quota4.RealizarAgendamento(agendamento2.Id);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota4);

            var consultaQuotaRelatorio = ObjectFactory.GetInstance<IConsultaQuotaRelatorio>();
            RelatorioDeQuotaPlanejadoVersusRealizadoVm relatorioVm = consultaQuotaRelatorio.PlanejadoRealizado(
                new RelatorioAgendamentoFiltroVm
                {
                    CodigoTerminal = "1000"
                });

            Assert.AreEqual(2, relatorioVm.Quotas.Count);

            QuotaPlanejadoRealizadoVm planejadoRealizadoFornecedor1 = relatorioVm.Quotas
                .First(x => x.NomeDoFornecedor == fornecedor1.Codigo + " - " + fornecedor1.Nome);
            Assert.AreEqual(fornecedor1.Codigo + " - " + fornecedor1.Nome, planejadoRealizadoFornecedor1.NomeDoFornecedor);
            Assert.AreEqual("1000", planejadoRealizadoFornecedor1.CodigoTerminal);
            Assert.AreEqual(quota1.FluxoDeCarga.Descricao(),planejadoRealizadoFornecedor1.FluxoDeCarga);
            Assert.AreEqual(300, planejadoRealizadoFornecedor1.Quota);
            Assert.AreEqual(30, planejadoRealizadoFornecedor1.PesoRealizado);

            QuotaPlanejadoRealizadoVm planejadoRealizadoFornecedor2 = relatorioVm.Quotas
                .First(x => x.NomeDoFornecedor == fornecedor2.Codigo + " - " + fornecedor2.Nome);
            Assert.AreEqual(fornecedor2.Codigo + " - " +  fornecedor2.Nome, planejadoRealizadoFornecedor2.NomeDoFornecedor);
            Assert.AreEqual("1000", planejadoRealizadoFornecedor2.CodigoTerminal);
            Assert.AreEqual(quota1.FluxoDeCarga.Descricao(), planejadoRealizadoFornecedor2.FluxoDeCarga);
            Assert.AreEqual(400, planejadoRealizadoFornecedor2.Quota);
            Assert.AreEqual(250, planejadoRealizadoFornecedor2.PesoRealizado);

            Assert.AreEqual(700, relatorioVm.Total.Quota);
            Assert.AreEqual(280, relatorioVm.Total.PesoRealizado);
        }

        [TestMethod]
        public void ConsultaPrevistoRealizadoFiltraPorFornecedorCorretamente()
        {
            //cria duas quotas, dois fornecedores. Os fornecedores tem agendamentos nas duas quotas. 
            //Apenas uma das quotas de cada fornecedor está realizada.
            RemoveQueries.RemoverQuotasCadastradas();

            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Terminal terminal = DefaultObjects.ObtemTerminalPadrao();

            var quota1 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor1, terminal, DateTime.Today.AddDays(1), 100);
            var quota2 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor1, terminal, DateTime.Today.AddDays(2), 200);
            var quota3 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor2, terminal, DateTime.Today.AddDays(1), 150);
            var quota4 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor2, terminal, DateTime.Today.AddDays(2), 250);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota2);
            DefaultPersistedObjects.PersistirQuota(quota3);
            DefaultPersistedObjects.PersistirQuota(quota4);

            AgendamentoDeCarregamento agendamento1 = quota1.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm { Placa = "IOQ3434", Peso = 30 });
            AgendamentoDeCarregamento agendamento2 = quota4.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm { Placa = "IOQ3435", Peso = 250 });

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota4);

            quota1.RealizarAgendamento(agendamento1.Id);
            quota4.RealizarAgendamento(agendamento2.Id);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota4);

            var consultaQuotaRelatorio = ObjectFactory.GetInstance<IConsultaQuotaRelatorio>();
            RelatorioDeQuotaPlanejadoVersusRealizadoVm relatorioVm = consultaQuotaRelatorio.PlanejadoRealizado(
                new RelatorioAgendamentoFiltroVm
                {
                    CodigoTerminal = "1000",
                    CodigoFornecedor = fornecedor1.Codigo
                });

            Assert.AreEqual(1, relatorioVm.Quotas.Count);

        }

        [TestMethod]
        public void ConsultaPrevistoRealizadoPorDataRetornaOsValoresAgrupadosCorretamente()
        {
            //cria duas quotas, dois fornecedores. Os fornecedores tem agendamentos nas duas quotas. 
            //Apenas uma das quotas de cada fornecedor está realizada.
            RemoveQueries.RemoverQuotasCadastradas();

            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Terminal terminal = DefaultObjects.ObtemTerminalPadrao();

            var quota1 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor1, terminal, DateTime.Today.AddDays(1), 100);
            var quota2 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor1, terminal, DateTime.Today.AddDays(2), 200);
            var quota3 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor2, terminal, DateTime.Today.AddDays(1), 150);
            var quota4 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor2, terminal, DateTime.Today.AddDays(2), 250);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota2);
            DefaultPersistedObjects.PersistirQuota(quota3);
            DefaultPersistedObjects.PersistirQuota(quota4);

            AgendamentoDeCarregamento agendamento1 = quota1.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm { Placa = "IOQ3434", Peso = 30 });
            AgendamentoDeCarregamento agendamento2 = quota4.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm { Placa = "IOQ3435", Peso = 250 });

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota4);

            quota1.RealizarAgendamento(agendamento1.Id);
            quota4.RealizarAgendamento(agendamento2.Id);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota4);

            var consultaQuotaRelatorio = ObjectFactory.GetInstance<IConsultaQuotaRelatorio>();
            RelatorioDeQuotaPlanejadoVersusRealizadoPorDataVm relatorioVm = consultaQuotaRelatorio.PlanejadoRealizadoPorData(
                new RelatorioAgendamentoFiltroVm
                {
                    CodigoTerminal = "1000"
                });


            Assert.AreEqual(4, relatorioVm.Quotas.Count);

            //Assert Fornecedor 1 - Dia 1
            QuotaPlanejadoRealizadoPorDataVm planejadoRealizadoFornecedor1 = relatorioVm.Quotas
                .First(x => x.NomeDoFornecedor == fornecedor1.Codigo + " - " + fornecedor1.Nome 
                    && x.Data == DateTime.Today.AddDays(1).ToShortDateString());

            Assert.AreEqual(fornecedor1.Codigo + " - " + fornecedor1.Nome, planejadoRealizadoFornecedor1.NomeDoFornecedor);
            Assert.AreEqual("1000", planejadoRealizadoFornecedor1.CodigoTerminal);
            Assert.AreEqual(quota1.FluxoDeCarga.Descricao(), planejadoRealizadoFornecedor1.FluxoDeCarga);
            Assert.AreEqual(100, planejadoRealizadoFornecedor1.Quota);
            Assert.AreEqual(30, planejadoRealizadoFornecedor1.PesoRealizado);

            //Assert Fornecedor 1 - Dia 2
            planejadoRealizadoFornecedor1 = relatorioVm.Quotas
                .First(x => x.NomeDoFornecedor == fornecedor1.Codigo + " - " + fornecedor1.Nome
                    && x.Data == DateTime.Today.AddDays(2).ToShortDateString());

            Assert.AreEqual(fornecedor1.Codigo + " - " + fornecedor1.Nome, planejadoRealizadoFornecedor1.NomeDoFornecedor);
            Assert.AreEqual("1000", planejadoRealizadoFornecedor1.CodigoTerminal);
            Assert.AreEqual(quota1.FluxoDeCarga.Descricao(), planejadoRealizadoFornecedor1.FluxoDeCarga);
            Assert.AreEqual(200, planejadoRealizadoFornecedor1.Quota);
            Assert.AreEqual(0, planejadoRealizadoFornecedor1.PesoRealizado);

            //Assert Fornecedor 2 - Dia 1
            QuotaPlanejadoRealizadoVm planejadoRealizadoFornecedor2 = relatorioVm.Quotas
                .First(x => x.NomeDoFornecedor == fornecedor2.Codigo + " - " + fornecedor2.Nome
                && x.Data == DateTime.Today.AddDays(1).ToShortDateString());
            Assert.AreEqual(fornecedor2.Codigo + " - " + fornecedor2.Nome, planejadoRealizadoFornecedor2.NomeDoFornecedor);
            Assert.AreEqual("1000", planejadoRealizadoFornecedor2.CodigoTerminal);
            Assert.AreEqual(quota1.FluxoDeCarga.Descricao(), planejadoRealizadoFornecedor2.FluxoDeCarga);
            Assert.AreEqual(150, planejadoRealizadoFornecedor2.Quota);
            Assert.AreEqual(0, planejadoRealizadoFornecedor2.PesoRealizado);

            //Assert Fornecedor 2 - Dia 2
            planejadoRealizadoFornecedor2 = relatorioVm.Quotas
                .First(x => x.NomeDoFornecedor == fornecedor2.Codigo + " - " + fornecedor2.Nome
                && x.Data == DateTime.Today.AddDays(2).ToShortDateString());
            Assert.AreEqual(fornecedor2.Codigo + " - " + fornecedor2.Nome, planejadoRealizadoFornecedor2.NomeDoFornecedor);
            Assert.AreEqual("1000", planejadoRealizadoFornecedor2.CodigoTerminal);
            Assert.AreEqual(quota1.FluxoDeCarga.Descricao(), planejadoRealizadoFornecedor2.FluxoDeCarga);
            Assert.AreEqual(250, planejadoRealizadoFornecedor2.Quota);
            Assert.AreEqual(250, planejadoRealizadoFornecedor2.PesoRealizado);

            Assert.AreEqual(700, relatorioVm.Total.Quota);
            Assert.AreEqual(280, relatorioVm.Total.PesoRealizado);

        }
    }
}

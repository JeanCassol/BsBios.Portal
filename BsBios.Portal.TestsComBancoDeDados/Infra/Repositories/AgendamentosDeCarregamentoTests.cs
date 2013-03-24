using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class AgendamentosDeCarregamentoTests: RepositoryTest
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
        public void ConsigoPersistirUmAgendamentoDeCarregamentoEConsultarPosteriormente()
        {
            AgendamentoDeCarregamento  agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(150);

            DefaultPersistedObjects.PersistirAgendamentoDeCarga(agendamento);

            var agendamentos = ObjectFactory.GetInstance<IAgendamentosDeCarga>();

            var agendamentoConsultado = (AgendamentoDeCarregamento) agendamentos.BuscaPorId(agendamento.Id);

            Assert.IsNotNull(agendamentoConsultado);
            Assert.AreEqual(agendamento.Material, agendamentoConsultado.Material);
            Assert.AreEqual(agendamento.Data, agendamentoConsultado.Data);
            Assert.AreEqual(agendamento.CodigoTerminal, agendamentoConsultado.CodigoTerminal);
            Assert.AreEqual(agendamento.Peso, agendamentoConsultado.Peso);
            Assert.AreEqual(agendamento.Placa, agendamentoConsultado.Placa);
            Assert.AreEqual(agendamento.Realizado, agendamentoConsultado.Realizado);

        }
    }
}

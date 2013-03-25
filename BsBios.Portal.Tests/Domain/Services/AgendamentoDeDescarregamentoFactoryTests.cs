using System;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class AgendamentoDeDescarregamentoFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(AgendamentoDeDescarregamentoSemNotaFiscalException))]
        public void QuandoCriarUmAgendamentoSemNotaDeveGerarExcecao()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            var factory = new AgendamentoDeDescarregamentoFactory();
            factory.Construir(quota, "IOQ5338");
        }

        [TestMethod]
        public void QuandoCrioUmAgendamentoAsPropriedadesFicamCorretas()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            var factory = new AgendamentoDeDescarregamentoFactory();
            factory.AdicionarNotaFiscal(DefaultObjects.ObtemNotaFiscalVmPadrao());
            var agendamento = (AgendamentoDeDescarregamento)  factory.Construir(quota, "IOQ5338");
            Assert.AreEqual(DateTime.Today, agendamento.Quota.Data);
            Assert.AreEqual("1000", agendamento.Quota.CodigoTerminal);
            Assert.AreEqual("IOQ5338", agendamento.Placa);
            Assert.AreEqual(1, agendamento.NotasFiscais.Count);
        }

        [TestMethod]
        public void PesoTotalDoDescarregamentoEaSomaDosPesosDasNotas()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            var factory = new AgendamentoDeDescarregamentoFactory();
            NotaFiscalVm nota1 = DefaultObjects.ObtemNotaFiscalVmPadrao();
            nota1.Peso = 120;
            NotaFiscalVm nota2 = DefaultObjects.ObtemNotaFiscalVmPadrao();
            nota2.Peso = 140;

            factory.AdicionarNotaFiscal(nota1);
            factory.AdicionarNotaFiscal(nota2);
            var agendamento = (AgendamentoDeDescarregamento)factory.Construir(quota, "IOQ5338");
            Assert.AreEqual(260, agendamento.PesoTotal);
            
        }
    }
}

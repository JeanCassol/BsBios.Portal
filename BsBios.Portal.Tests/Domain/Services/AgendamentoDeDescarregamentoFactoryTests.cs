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
            var factory = new AgendamentoDeDescarregamentoFactory();
            factory.Construir(DateTime.Today, "IOQ5338");
        }

        [TestMethod]
        public void QuandoCrioUmAgendamentoAsPropriedadesFicamCorretas()
        {
            var factory = new AgendamentoDeDescarregamentoFactory();
            factory.AdicionarNotaFiscal(DefaultObjects.ObtemNotaFiscalVmPadrao());
            var agendamento = (AgendamentoDeDescarregamento)  factory.Construir(DateTime.Today, "IOQ5338");
            Assert.AreEqual(DateTime.Today, agendamento.Data);
            Assert.AreEqual("IOQ5338", agendamento.Placa);
            Assert.AreEqual(1, agendamento.NotasFiscais.Count);
        }

        [TestMethod]
        public void PesoTotalDoDescarregamentoEaSomaDosPesosDasNotas()
        {
            var factory = new AgendamentoDeDescarregamentoFactory();
            NotaFiscalVm nota1 = DefaultObjects.ObtemNotaFiscalVmPadrao();
            nota1.Peso = 120;
            NotaFiscalVm nota2 = DefaultObjects.ObtemNotaFiscalVmPadrao();
            nota2.Peso = 140;

            factory.AdicionarNotaFiscal(nota1);
            factory.AdicionarNotaFiscal(nota2);
            var agendamento = (AgendamentoDeDescarregamento)factory.Construir(DateTime.Today, "IOQ5338");
            Assert.AreEqual(260, agendamento.PesoTotal);
            
        }
    }
}

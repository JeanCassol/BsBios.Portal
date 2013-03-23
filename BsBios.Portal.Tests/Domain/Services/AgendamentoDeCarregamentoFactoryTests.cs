using System;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class AgendamentoDeCarregamentoFactoryTests
    {
        [TestMethod]
        [ExpectedException((typeof(AgendamentoDeCarregamentoSemPesoException)))]
        public void QuandoCriarCarregamentoSemInformarPesoDeveGerarExcecao()
        {
            var factory = new AgendamentoDeCarregamentoFactory(0);
            factory.Construir(DateTime.Today, "IOQ5335");
        }

        [TestMethod]
        [ExpectedException(typeof (AgendamentoDeCarregamentoComNotaFiscalException))]
        public void QuandoInformarNotaParaUmAgendamentoDeCarregamentoDeveGerarExcecao()
        {
            var factory = new AgendamentoDeCarregamentoFactory(150);
            factory.AdicionarNotaFiscal(DefaultObjects.ObtemNotaFiscalVmPadrao());
            factory.Construir(DateTime.Today, "IOQ5335");
            
        }

        [TestMethod]
        public void QuandoCriaUmAgendamentoDeCarregamentoAsPropriedadesFicamCorretas()
        {
            var factory = new AgendamentoDeCarregamentoFactory(150);
            var agendamento = (AgendamentoDeCarregamento) factory.Construir(DateTime.Today, "IOQ5335");
            Assert.AreEqual(DateTime.Today, agendamento.Data);
            Assert.AreEqual("IOQ5335",agendamento.Placa);
            Assert.AreEqual(150, agendamento.Peso);
            Assert.AreEqual(Enumeradores.MaterialDeCarga.Farelo , agendamento.Material);
            
        }

        [TestMethod]
        public void QuandoCriaUmAgendamentoDeCarregamentoIniciaComoNaoRealizado()
        {
            var factory = new AgendamentoDeCarregamentoFactory(150);
            var agendamento = (AgendamentoDeCarregamento)factory.Construir(DateTime.Today, "IOQ5335");
            Assert.IsFalse(agendamento.Realizado);
        }

        [TestMethod]
        public void PesoTotalDoCarregamentoEoPesoAgendado()
        {
            var factory = new AgendamentoDeCarregamentoFactory(150);
            var agendamento = (AgendamentoDeCarregamento)factory.Construir(DateTime.Today, "IOQ5335");

            Assert.AreEqual(150, agendamento.PesoTotal);
            
        }

    }
}

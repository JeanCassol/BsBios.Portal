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
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            var factory = new AgendamentoDeCarregamentoFactory("Motorista", "Destino",0);
            factory.Construir(quota, "IOQ5335");
        }

        [TestMethod]
        [ExpectedException(typeof (AgendamentoDeCarregamentoComNotaFiscalException))]
        public void QuandoInformarNotaParaUmAgendamentoDeCarregamentoDeveGerarExcecao()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            var factory = new AgendamentoDeCarregamentoFactory("Motorista", "Destino", 150);
            factory.AdicionarNotaFiscal(DefaultObjects.ObtemNotaFiscalVmPadrao());
            factory.Construir(quota, "IOQ5335");
            
        }

        [TestMethod]
        public void QuandoCriaUmAgendamentoDeCarregamentoAsPropriedadesFicamCorretas()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            var factory = new AgendamentoDeCarregamentoFactory("Motorista", "Destino", 150);
            var agendamento = (AgendamentoDeCarregamento) factory.Construir(quota, "IOQ5335");
            Assert.AreEqual(DateTime.Today, agendamento.Quota.Data);
            Assert.AreEqual("1000", agendamento.Quota.Terminal.Codigo);
            Assert.AreEqual("IOQ5335",agendamento.Placa);
            Assert.AreEqual(150, agendamento.Peso);
            Assert.AreEqual("Farelo", agendamento.Quota.Material.Descricao);
            Assert.AreEqual("Motorista", agendamento.Motorista);
            Assert.AreEqual("Destino", agendamento.Destino);
            
        }

        [TestMethod]
        public void QuandoCriaUmAgendamentoDeCarregamentoIniciaComoNaoRealizado()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();

            var factory = new AgendamentoDeCarregamentoFactory("Motorista", "Destino", 150);
            var agendamento = (AgendamentoDeCarregamento)factory.Construir(quota, "IOQ5335");
            Assert.IsFalse(agendamento.Realizado);
        }

        [TestMethod]
        public void PesoTotalDoCarregamentoEoPesoAgendado()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            var factory = new AgendamentoDeCarregamentoFactory("Motorista", "Destino", 150);
            var agendamento = (AgendamentoDeCarregamento)factory.Construir(quota, "IOQ5335");

            Assert.AreEqual(150, agendamento.PesoTotal);
            
        }

    }
}

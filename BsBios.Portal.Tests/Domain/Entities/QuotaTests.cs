using System;
using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class QuotaTests
    {
        [TestMethod]
        public void QuandoCrioUmaQuotaAsPropriedadesFicamCorretas()
        {
            Fornecedor transportadora = DefaultObjects.ObtemTransportadoraPadrao();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, transportadora, "1000", DateTime.Today, 1000);

            Assert.AreSame(transportadora, quota.Fornecedor);
            Assert.AreEqual("1000", quota.CodigoTerminal);
            Assert.AreEqual(DateTime.Today, quota.Data);
            Assert.AreEqual(Enumeradores.MaterialDeCarga.Soja,quota.Material);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Descarregamento, quota.FluxoDeCarga);
            Assert.AreEqual(1000, quota.PesoTotal);
        }

        [TestMethod]
        public void QuandoCriuoUmaQuotaDeSojaOFluxoEDescarregamento()
        {
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemFornecedorPadrao(), "1000", DateTime.Today, 1000);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Descarregamento, quota.FluxoDeCarga);
        }

        [TestMethod]
        public void QuandoCriuoUmaQuotaDeFareloOFluxoECarregamento()
        {
            var quota = new Quota(Enumeradores.MaterialDeCarga.Farelo, DefaultObjects.ObtemFornecedorPadrao(), "1000", DateTime.Today, 1000);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Carregamento, quota.FluxoDeCarga);
        }

        [TestMethod]
        public void QuandoAdicionoAgendamentosCalculaOPesoAgendadoCorretamente()
        {
            //peso total é 850
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
                {
                    IdQuota = quota.Id,
                    IdAgendamento = 0,
                    Placa = "IMN2420",
                    NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(180),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(230)
                        }
                });
            Assert.AreEqual(410, quota.PesoAgendado);
        }

        [TestMethod]
        [ExpectedException(typeof(PesoAgendadoSuperiorAoPesoDaQuotaException))]
        public void QuandoPesoAgendadoSuperiorPesoDaQuotaDeveDispararExcecao()
        {
            //peso total é 850
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(450),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(401)
                        }
            });
        }
    }
}

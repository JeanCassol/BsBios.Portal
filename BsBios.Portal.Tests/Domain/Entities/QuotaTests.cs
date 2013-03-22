using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
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

            Assert.AreSame(transportadora, quota.Transportadora);
            Assert.AreEqual("1000", quota.Terminal);
            Assert.AreEqual(DateTime.Today, quota.Data);
            Assert.AreEqual(Enumeradores.MaterialDeCarga.Soja,quota.Material);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Descarregamento, quota.FluxoDeCarga);
            Assert.AreEqual(1000, quota.Peso);
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

    }
}

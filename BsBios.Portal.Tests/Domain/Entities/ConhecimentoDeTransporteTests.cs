using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class ConhecimentoDeTransporteTests
    {
        [TestMethod]
        public void UmConhecimentoDeTransporteIniciaComEstadoSemOrdemDeTransporte()
        {
            var conhecimentoDeTransporte = new ConhecimentoDeTransporte("11","1","1",DateTime.Today,"1","1900","2000", 1000M, 100000);
            Assert.AreEqual(Enumeradores.StatusDoConhecimentoDeTransporte.SemOrdemDeTransporte, conhecimentoDeTransporte.Status);
        }

        [TestMethod]
        public void PesoTotalDoConhecimentoDeveSerConvertidoDeQuilosParaToneladas()
        {
            var conhecimentoDeTransporte = new ConhecimentoDeTransporte("11", "1", "1", DateTime.Today, "1", "1900", "2000", 1000M, 100000);
            Assert.AreEqual(100, conhecimentoDeTransporte.PesoTotalDaCargaEmToneladas);
        }

    }
}

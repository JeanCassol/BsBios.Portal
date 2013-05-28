using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services;
using BsBios.Portal.Domain.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class CalculadorDeBaseDeCalculoFactoryTests
    {
        [TestMethod]
        public void CalculadorDeBaseDeCalculoParaIpiDeMateriaPrimaEInstanciadoCorretamente()
        {
            var factory = new CalculadorDeBaseDeCalculoFactory();
            var calculador = factory.Construir(Enumeradores.TipoDeImposto.Ipi, new Produto("prd0001", "produto 0001", "ROH"));
            Assert.IsInstanceOfType(calculador, typeof(CalculadorDeBaseDeCalculoComCreditoDeImpostos));
        }

        [TestMethod]
        public void CalculadorDeBaseDeCalculoParaIpiDeOutrosProdutosEInstanciadoCorretamente()
        {
            var factory = new CalculadorDeBaseDeCalculoFactory();
            var calculador = factory.Construir(Enumeradores.TipoDeImposto.Ipi, new Produto("prd0001", "produto 0001", "HIBE"));
            Assert.IsInstanceOfType(calculador, typeof(CalculadorDeBaseDeCalculoPadrao));
        }
    }
}

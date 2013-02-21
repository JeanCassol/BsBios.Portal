using System;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class PedidoDeCotacaoTests
    {
        [TestMethod]
        public void QuandoCrioUmPedidoDeCotacaoAsPropriedadesFicamCorretas()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            var dataLimiteDeRetorno = DateTime.Today.AddDays(10);
            var pedidoDeCotacao = new PedidoDeCotacao(requisicaoDeCompra, dataLimiteDeRetorno);
            
            Assert.AreEqual( "REQ0001", pedidoDeCotacao.RequisicaoDeCompra.Numero);
            Assert.AreEqual("ITEM001",pedidoDeCotacao.RequisicaoDeCompra.NumeroItem);

            Assert.AreEqual(dataLimiteDeRetorno, pedidoDeCotacao.DataLimiteDeRetorno);
        }

        [TestMethod]
        public void QuandoCrioUmPedidoDeCotacaoIniciaNoEstadoNaoIniciado()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            var dataLimiteDeRetorno = DateTime.Today.AddDays(10);
            var pedidoDeCotacao = new PedidoDeCotacao(requisicaoDeCompra, dataLimiteDeRetorno);
            Assert.AreEqual(Enumeradores.StatusPedidoCotacao.NaoIniciado, pedidoDeCotacao.Status);            
        }
    }
}

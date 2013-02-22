using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class ProcessoDeCotacaoTests
    {
        [TestMethod]
        public void QuandoCrioUmPedidoDeCotacaoAsPropriedadesFicamCorretas()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            //var dataLimiteDeRetorno = DateTime.Today.AddDays(10);
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra);
            
            Assert.AreEqual( "REQ0001", processoDeCotacao.RequisicaoDeCompra.Numero);
            Assert.AreEqual("ITEM001",processoDeCotacao.RequisicaoDeCompra.NumeroItem);

            Assert.AreEqual(0, processoDeCotacao.Fornecedores.Count);
            Assert.IsNull(processoDeCotacao.DataLimiteDeRetorno);

        }

        [TestMethod]
        public void QuandoCrioUmPedidoDeCotacaoIniciaNoEstadoNaoIniciado()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            //var dataLimiteDeRetorno = DateTime.Today.AddDays(10);
            var pedidoDeCotacaoDeMaterial = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra);
            Assert.AreEqual(Enumeradores.StatusPedidoCotacao.NaoIniciado, pedidoDeCotacaoDeMaterial.Status);            
        }
        [TestMethod]
        public void QuandoAtualizoDadosComplementaresDeUmProcessoDeCotacaoAsPropriedadesSaoAlteradas()
        {
            Assert.Fail();
            
        }

        [TestMethod]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        {
            Assert.Fail();
        }
        [TestMethod]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelAtualizarOsDadosComplementares()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void QuandoAtualizoAListaDeFornecedoresDoProcessoDeCotacaoOsFornecedoresDoProcessoFicamIguaisAListaRecebida()
        {
            Assert.Fail();
            
        }

        [TestMethod]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarFornecedores()
        {
            Assert.Fail();
            
        }
        [TestMethod]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelAtualizarFornecedores()
        {
            Assert.Fail();

        }

    }
}

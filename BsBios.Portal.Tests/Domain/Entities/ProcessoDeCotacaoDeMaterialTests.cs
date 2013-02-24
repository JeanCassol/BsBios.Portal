using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class ProcessoDeCotacaoTests
    {
        [TestMethod]
        public void QuandoCrioUmProcessoDeCotacaoAsPropriedadesFicamCorretas()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra, produto, 100);
            
            Assert.AreEqual("REQ0001", processoDeCotacao.RequisicaoDeCompra.Numero);
            Assert.AreEqual("00001", processoDeCotacao.RequisicaoDeCompra.NumeroItem);
            Assert.AreEqual("PROD0001", processoDeCotacao.Produto.Codigo);
            Assert.AreEqual(100, processoDeCotacao.Quantidade);

            Assert.AreEqual(0, processoDeCotacao.Fornecedores.Count);
            Assert.IsNull(processoDeCotacao.DataLimiteDeRetorno);

        }

        [TestMethod]
        public void QuandoCrioUmProcessoDeCotacaoIniciaNoEstadoNaoIniciado()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra, produto, 100);
            Assert.AreEqual(Enumeradores.StatusPedidoCotacao.NaoIniciado, processoDeCotacao.Status);            
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

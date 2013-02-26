using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class CotacaoTests
    {
        [TestMethod]
        public void QuandoCrioUmaCotacaoAsPropriedadesSaoInicializadasCorretamente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var cotacao = new Cotacao(processoDeCotacao, fornecedor);

            var processoDeCotacaoDeMaterial = (ProcessoDeCotacaoDeMaterial) cotacao.ProcessoDeCotacao;

            Assert.AreEqual("REQ0001", processoDeCotacaoDeMaterial.RequisicaoDeCompra.Numero);
            Assert.AreEqual("00001", processoDeCotacaoDeMaterial.RequisicaoDeCompra.NumeroItem);
            Assert.AreEqual("FORNEC0001", cotacao.Fornecedor.Codigo);
            Assert.IsNull(cotacao.ValorUnitario);
            Assert.IsNull(cotacao.QuantidadeAdquirida);
        }

        
    }
}

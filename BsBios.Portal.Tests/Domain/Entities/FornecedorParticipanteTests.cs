using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class FornecedorParticipanteTests
    {
        [TestMethod]
        public void QuandoInstanciaUmFornecedorParticipanteAsPropriedadesSaoCriadasCorretamente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedorParticipante = processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id, fornecedorParticipante.ProcessoDeCotacao.Id);
            Assert.AreEqual(fornecedor.Codigo, fornecedorParticipante.Fornecedor.Codigo);
        }
    }
}

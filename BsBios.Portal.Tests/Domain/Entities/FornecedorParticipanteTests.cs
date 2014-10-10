using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class FornecedorParticipanteTests
    {
        [TestMethod]
        public void QuandoAdicionoUmFornecedorNoProcessoDeCotacaoARespostaIniciaComoPendente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedorParticipante = processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);

            Assert.AreEqual(Enumeradores.RespostaDaCotacao.Pendente, fornecedorParticipante.Resposta);

        }
    }
}

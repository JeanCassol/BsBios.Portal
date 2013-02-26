using BsBios.Portal.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class CondicaoDePagamentoTests
    {
        [TestMethod]
        public void QuandoCrioUmaCondicaoDePagamentoConsigoAcessarAsPropriedades()
        {
            var condiaoDePagamento = new CondicaoDePagamento("C001", "CONDICAO 0001");
            Assert.AreEqual("C001", condiaoDePagamento.Codigo);
            Assert.AreEqual("CONDICAO 0001", condiaoDePagamento.Descricao);
        }
        [TestMethod]
        public void QuandoAlteroADescricaoDaCondicaoDePagamentoAPropriedadeFicaComONovoValor()
        {
            var condiaoDePagamento = new CondicaoDePagamento("C001", "CONDICAO 0001");
            condiaoDePagamento.AtualizarDescricao("CONDICAO 0001 atualizada");
            Assert.AreEqual("CONDICAO 0001 atualizada", condiaoDePagamento.Descricao);
        }

    }

}

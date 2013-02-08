using System;
using BsBios.Portal.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class CondicaoDePagamentoTests
    {
        [TestMethod]
        public void QuandoCrioUmaCondicaoDePagamentoConsigoAcessarAsPropriedades()
        {
            var condiaoDePagamento = new CondicaoDePagamento("COND0001", "CONDICAO 0001");
            Assert.AreEqual("COND0001", condiaoDePagamento.CodigoSap);
            Assert.AreEqual("CONDICAO 0001", condiaoDePagamento.Descricao);
        }
    }

}

using BsBios.Portal.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class IncotermTests
    {
        [TestMethod]
        public void QuandoCrioUmIncotermConsigoAcessarAsPropriedades()
        {
            var incoterm = new Incoterm("001", "INCOTERM 001");
            Assert.AreEqual("001", incoterm.Codigo);
            Assert.AreEqual("INCOTERM 001", incoterm.Descricao);
        }

        [TestMethod]
        public void QuandoAlteroADescricaoDoIncotermAPropriedadeFicaComONovoValor()
        {
            var incoterm = new Incoterm("001", "INCOTERM 001");
            incoterm.AtualizaDescricao("INCOTERM 001 atualizado");
            Assert.AreEqual("INCOTERM 001 atualizado", incoterm.Descricao);
        }
    }

}

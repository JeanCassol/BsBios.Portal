using BsBios.Portal.Common;
using BsBios.Portal.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class ImpostoTests
    {
        [TestMethod]
        public void QuandoCrioUmImpostoAsPropriedadesFicamCorretas()
        {
            var imposto = new Imposto(Enumeradores.TipoDeImposto.Icms, 10, 100);
            Assert.AreEqual(Enumeradores.TipoDeImposto.Icms, imposto.Tipo);
            Assert.AreEqual(10, imposto.Aliquota);
            Assert.AreEqual(100, imposto.Valor);
        }

        [TestMethod]
        public void QuandoAtualizoImpostoAsPropriedadesFicamCorretas()
        {
            var imposto = new Imposto(Enumeradores.TipoDeImposto.Icms, 10, 100);
            imposto.Atualizar(20, 200);
            Assert.AreEqual(20, imposto.Aliquota);
            Assert.AreEqual(200, imposto.Valor);
            
        }
    }
}

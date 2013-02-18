using BsBios.Portal.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class IvaTests
    {
        [TestMethod]
        public void QuandoCrioUmIvaConsigoAcessarAsPropriedades()
        {
            var iva = new Iva("01", "IVA 01");
            Assert.AreEqual("01", iva.Codigo);
            Assert.AreEqual("IVA 01", iva.Descricao);
        }
    }

}

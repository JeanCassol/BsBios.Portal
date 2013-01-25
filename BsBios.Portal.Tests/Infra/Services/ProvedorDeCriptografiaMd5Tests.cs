using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class ProvedorDeCriptografiaMd5Tests
    {
        [TestMethod]
        public void QuandoCriptografarUmaStringDeveRetornarUmaStringCom24CaracteresDiferenteDaStringDeEntrada()
        {
            var provedor = new ProvedorDeCriptografiaMd5();
            var resultado = provedor.Criptografar("123456");
            Assert.AreEqual(24,resultado.Length);
            Assert.AreNotEqual("123456",resultado);
        }
    }
}

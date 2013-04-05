using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Infra.Services.Implementations;
using BsBios.Portal.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Services
{
    [TestClass]
    public class GeradorDeEmailDeAberturaDeProcessoDeCotacaoFactoryTests
    {
        [TestMethod]
        public void FactoryDoGeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFreteFunciona()
        {
            var factory = new GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFreteFactory();
            var geradorDeEmail = factory.Construir();

            Assert.IsNotNull(geradorDeEmail);
        }

        [TestMethod]
        public void FactoryDoGeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterialFunciona()
        {
            var factory = new GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterialFactory();
            var geradorDeEmail = factory.Construir();

            Assert.IsNotNull(geradorDeEmail);
        }

    }
}

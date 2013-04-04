using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Services
{
    [TestClass]
    public class AberturaDeProcessoDeCotacaoServiceFactoryTests
    {
        [TestMethod]
        public void FactoryDoServicoDeAberturaDoProcessoDeCotacaoDeFreteFunciona()
        {
            var factory = new AberturaDeProcessoDeCotacaoDeFreteServiceFactory();
            var aberturaService = factory.Construir();

            Assert.IsNotNull(aberturaService);
        }

        [TestMethod]
        public void FactoryDoServicoDeAberturaDoProcessoDeCotacaoDeMaterialFunciona()
        {
            var factory = new AberturaDeProcessoDeCotacaoDeMaterialServiceFactory();
            var aberturaService = factory.Construir();

            Assert.IsNotNull(aberturaService);
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeAberturaDeProcessoDeCotacaoDeFrete()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeFreteAberturaController>();
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeAberturaDeProcessoDeCotacaoDeMaterial()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeMaterialAberturaController>();
            Assert.IsNotNull(controller);
        }

    }
}

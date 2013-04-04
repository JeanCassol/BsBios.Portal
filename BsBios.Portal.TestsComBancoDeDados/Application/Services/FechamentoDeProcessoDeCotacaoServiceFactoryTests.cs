using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Services
{
    [TestClass]
    public class FechamentoDeProcessoDeCotacaoServiceFactoryTests
    {
        [TestMethod]
        public void FactoryDoServicoDeFechamentoDoProcessoDeCotacaoDeFreteFunciona()
        {
            var factory = new FechamentoDeProcessoDeCotacaoDeFreteServiceFactory();
            var service = factory.Construir();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void FactoryDoServicoDeFechamentoDoProcessoDeCotacaoDeMaterialFunciona()
        {
            var factory = new FechamentoDeProcessoDeCotacaoDeMaterialServiceFactory();
            var service = factory.Construir();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeFechamentoDeProcessoDeCotacaoDeFrete()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeFreteFechamentoController>();
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeAberturaDeFechamentoDeCotacaoDeMaterial()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeMaterialFechamentoController>();
            Assert.IsNotNull(controller);
        }


    }
}

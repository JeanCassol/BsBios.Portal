using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaMunicipioTests
    {
        [TestMethod]
        public void ConsigoConsultarMunicipioInformandoParteDoNome()
        {
            var consultaMunicipio = ObjectFactory.GetInstance<IConsultaMunicipio>();
            var municipios = consultaMunicipio.NomeComecandoCom("torit");
            Assert.AreEqual(1, municipios.Count);
            var municipio = municipios.First();
            Assert.AreEqual("Toritama/PE", municipio.label);
            Assert.AreEqual("Toritama/PE", municipio.value);
            Assert.AreEqual("2615409", municipio.Codigo);
        }
    }
}

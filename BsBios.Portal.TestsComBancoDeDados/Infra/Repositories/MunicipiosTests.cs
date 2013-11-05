using System;
using System.Linq;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class MunicipiosTests: RepositoryTest
    {
        [TestMethod]
        public void ConsigoConsultarUmMunicipioInformandoParteDaDescricao()
        {
            var municipios = ObjectFactory.GetInstance<IMunicipios>();
             municipios.NomeComecandoCom("tor");

            var municipiosFiltrados = municipios.List();

            Assert.AreEqual(2, municipiosFiltrados.Count);

            Assert.AreEqual("Toropi", municipiosFiltrados.First().Nome);
            Assert.AreEqual("Torres", municipiosFiltrados.Last().Nome);

        }

        [TestMethod]
        public void ConsigoConsultarUmMunicipioPeloCodigo()
        {
            var municipios = ObjectFactory.GetInstance<IMunicipios>();
            Municipio municipio = municipios.BuscaPeloCodigo("4322004");
            Assert.AreEqual("Triunfo", municipio.Nome);
        }

        [TestMethod]
        public void ConsigoConsultarDoisMunicipiosPeloCodigo()
        {
            var municipios = ObjectFactory.GetInstance<IMunicipios>();
            Municipio municipio1 = municipios.BuscaPeloCodigo("4322251");
            Assert.AreEqual("Tupandi", municipio1.Nome);

            Municipio municipio2 = municipios.BuscaPeloCodigo("4322509");
            Assert.AreEqual("Vacaria", municipio2.Nome);
            
        }
    }
}

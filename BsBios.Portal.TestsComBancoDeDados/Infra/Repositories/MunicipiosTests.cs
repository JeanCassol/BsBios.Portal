using System;
using System.Linq;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
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
             municipios.NomeComecandoCom("torr");

            var municipiosFiltrados = municipios.List();

            Assert.AreEqual(3, municipiosFiltrados.Count);

            Assert.IsTrue(municipiosFiltrados.Any(x => x.Nome == "Torre de Pedra"));
            Assert.IsTrue(municipiosFiltrados.Any(x => x.Nome == "Torres"));
            Assert.IsTrue(municipiosFiltrados.Any(x => x.Nome == "Torrinha"));

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

        [TestMethod]
        public void NaoExistemMunicipiosComMenosDeTresCaracteres()
        {
            //se este teste falhar tem que mudar configuração da busca de municipios, que faz a consulta apenas após o terceiro caracter digitado
            var municipios = ObjectFactory.GetInstance<IMunicipios>();

            int resultado = (from municipio in municipios.GetQuery()
                where municipio.Nome.Length < 3
                select municipio).Count();

            Assert.AreEqual(0, resultado);

        }
    }
}

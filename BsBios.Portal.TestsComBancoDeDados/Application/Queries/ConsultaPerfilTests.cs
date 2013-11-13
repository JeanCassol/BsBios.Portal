using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaPerfilTests
    {
        [TestMethod]
        public void ConsigoObterOValorEaDescricaoDeTodosOsItensDoEnumPerfil()
        {
            var consultaPerfil = ObjectFactory.GetInstance<IConsultaPerfil>();
            IList<PerfilVm> perfis = consultaPerfil.Listar();
            Assert.AreEqual(8, perfis.Count);
        }
    }
}

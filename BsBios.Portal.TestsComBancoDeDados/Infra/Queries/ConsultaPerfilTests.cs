using System.Collections.Generic;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Queries
{
    [TestClass]
    public class ConsultaPerfilTests
    {
        [TestMethod]
        public void ConsigoObterOValorEaDescricaoDeTodosOsItensDoEnumPerfil()
        {
            var consultaPerfil = ObjectFactory.GetInstance<IConsultaPerfil>();
            IList<PerfilVm> perfis = consultaPerfil.Listar();
            Assert.AreEqual(7, perfis.Count);
        }
    }
}

using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Queries
{
    [TestClass]
    public class ConsultaUsuarioTests
    {
        [TestMethod]
        public void QuandoConsultaPerfisDoUsuarioRetornaListaDePerfis()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.AgendadorDeCargas); 
            usuario.AdicionarPerfil(Enumeradores.Perfil.CompradorLogistica);

            DefaultPersistedObjects.PersistirUsuario(usuario);

            var consultaUsuario = ObjectFactory.GetInstance<IConsultaUsuario>();

            IList<PerfilVm> perfisVm = consultaUsuario.PerfisDoUsuario(usuario.Login);

            Assert.AreEqual(2, perfisVm.Count);

        }
    }
}

using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
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

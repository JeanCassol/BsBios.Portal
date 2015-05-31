using System.Linq;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    /// <summary>
    /// Summary description for ConsultaRelatorioDeUsuarioTests
    /// </summary>
    [TestClass]
    public class ConsultaRelatorioDeUsuarioTests : RepositoryTest
    {
        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void QuandoExecutarConsultaDoRelatorioDeUsuarioDeveRetornarListagem()
        {
            RemoveQueries.RemoverUsuariosCadastrados();

            //criar usuário
            Usuario usuarioNovo = DefaultObjects.ObtemUsuarioPadrao();
            usuarioNovo.AdicionarPerfil(Enumeradores.Perfil.CompradorLogistica);
            usuarioNovo.AdicionarPerfil(Enumeradores.Perfil.ConferidorDeCargas);
            DefaultPersistedObjects.PersistirUsuario(usuarioNovo);

            UnitOfWorkNh.Session.Clear();

            var consultaDeRelatorioDeUsuario = ObjectFactory.GetInstance<IConsultaDeRelatorioDeUsuario>();
            var filtro = new RelatorioDeUsuarioFiltroVm {Status = 1};
            IList<RelatorioDeUsuarioListagemVm> usuarios = consultaDeRelatorioDeUsuario.Listar(filtro).ToList();

            Assert.IsNotNull(usuarios);

            Assert.AreEqual(1, usuarios.Count());

            RelatorioDeUsuarioListagemVm usuario = usuarios.First();

            Assert.AreEqual("Comprador Logística - Conferidor de Cargas", usuario.Perfis);
        }

    }
}

using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class UsuariosTests: RepositoryTest
    {
        private readonly IUsuarios _usuarios;

        public UsuariosTests()
        {
            _usuarios = ObjectFactory.GetInstance<IUsuarios>(); ;
        }

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverUsuariosCadastrados();
        }




        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        /// <summary>
        /// nesse mesmo teste consigo testar os métodos Save e BuscaPorId
        /// </summary>
        [TestMethod]
        public void QuandoPersistoUmUsuarioComSucessoPossoConsultarOMesmoUsuario()
        {
            UnitOfWorkNh.BeginTransaction();

            var usuarioNovo = new Usuario("Mauro Leal", "mauroscl", "123", "mauro.leal@fusionconsultoria.com.br", Enumeradores.Perfil.Comprador);
            _usuarios.Save(usuarioNovo);

            UnitOfWorkNh.Commit();

            Usuario usuarioConsulta = _usuarios.BuscaPorId(usuarioNovo.Id);

            Assert.IsNotNull(usuarioConsulta);
            Assert.AreEqual(usuarioNovo.Id, usuarioConsulta.Id);
        }

        [TestMethod]
        public void QuandoBuscarUsuarioPorLoginComLoginInexistenteDeveRetornarNull()
        {
            Usuario usuario = _usuarios.BuscaPorLogin("inexistente");
            Assert.IsNull(usuario);
        }

        [TestMethod]
        public void QuandoBuscarUsuarioPorLoginComLoginExistenteDeveRetornarUsuario()
        {
            Usuario usuario = _usuarios.BuscaPorLogin("mauroscl");
            Assert.IsNotNull(usuario);
            Assert.AreEqual("mauroscl",usuario.Login);
        }
    }
}

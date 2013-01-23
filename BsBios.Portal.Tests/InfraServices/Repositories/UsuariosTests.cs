using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.InfraServices.Repositories
{
    [TestClass]
    public class UsuariosTests: RepositoryTest
    {
        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            RemoverUsuariosCadastrados();
        }

        private static void RemoverUsuariosCadastrados()
        {
            UnitOfWorkNh.Session.CreateSQLQuery("DELETE FROM USUARIO").ExecuteUpdate();
        }


        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void ConsigoCriarUmUsuario()
        {
            UnitOfWorkNh.BeginTransaction();

            var usuarios = ObjectFactory.GetInstance<IUsuarios>();
            var usuarioNovo = new Usuario("Mauro Leal", "mauroscl", "123", "mauro.leal@fusionconsultoria.com.br");
            usuarios.Save(usuarioNovo);

            UnitOfWorkNh.Commit();

            Usuario usuarioConsulta = usuarios.BuscaPorId(usuarioNovo.Id).Single();

            Assert.IsNotNull(usuarioConsulta);
            Assert.AreEqual(usuarioNovo.Id, usuarioConsulta.Id);
        }
    }
}

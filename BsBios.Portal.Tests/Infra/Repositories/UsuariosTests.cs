using System;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class UsuariosTests: RepositoryTest
    {
        private static IUsuarios _usuarios;

        //public UsuariosTests()
        //{
        //    _usuarios = ObjectFactory.GetInstance<IUsuarios>(); ;
        //}

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverUsuariosCadastrados();
            _usuarios = ObjectFactory.GetInstance<IUsuarios>();
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


            try
            {
                UnitOfWorkNh.BeginTransaction();

                var usuarioNovo = new Usuario("Mauro Leal", "mauroscl", "mauro.leal@fusionconsultoria.com.br", Enumeradores.Perfil.Comprador);
                _usuarios.Save(usuarioNovo);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();
                throw;
            }

            Usuario usuarioConsulta = _usuarios.BuscaPorLogin("mauroscl");

            Assert.IsNotNull(usuarioConsulta);
            Assert.AreEqual("mauroscl", usuarioConsulta.Login);
            Assert.AreEqual("Mauro Leal", usuarioConsulta.Nome);
            Assert.IsNull(usuarioConsulta.Senha);
            Assert.AreEqual("mauro.leal@fusionconsultoria.com.br", usuarioConsulta.Email);
            Assert.AreEqual(Enumeradores.Perfil.Comprador, usuarioConsulta.Perfil);

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
            try
            {
                UnitOfWorkNh.BeginTransaction();

                var usuarioNovo = new Usuario("Mauro Leal", "usuario", "mauro.leal@fusionconsultoria.com.br", Enumeradores.Perfil.Comprador);
                _usuarios.Save(usuarioNovo);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                    
                throw;
            }
            Usuario usuario = _usuarios.BuscaPorLogin("usuario");
            Assert.IsNotNull(usuario);
            Assert.AreEqual("usuario",usuario.Login);
        }
    }
}

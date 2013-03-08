using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
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
            Usuario usuarioNovo;
            try
            {
                UnitOfWorkNh.BeginTransaction();

                usuarioNovo = DefaultObjects.ObtemUsuarioPadrao();
                _usuarios.Save(usuarioNovo);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();
                throw;
            }

            UnitOfWorkNh.Session.Clear();

            Usuario usuarioConsulta = _usuarios.BuscaPorLogin(usuarioNovo.Login);

            Assert.IsNotNull(usuarioConsulta);
            Assert.AreEqual(usuarioNovo.Login, usuarioConsulta.Login);
            Assert.AreEqual(usuarioNovo.Nome, usuarioConsulta.Nome);
            Assert.IsNull(usuarioConsulta.Senha);
            Assert.AreEqual(usuarioNovo.Email, usuarioConsulta.Email);
            Assert.AreEqual(usuarioNovo.Perfil, usuarioConsulta.Perfil);

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
            Usuario usuarioNovo;

            try
            {
                UnitOfWorkNh.BeginTransaction();

                usuarioNovo = DefaultObjects.ObtemUsuarioPadrao();
                _usuarios.Save(usuarioNovo);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                    
                throw;
            }
            Usuario usuario = _usuarios.BuscaPorLogin(usuarioNovo.Login);
            Assert.IsNotNull(usuario);
            Assert.AreEqual(usuarioNovo.Login,usuario.Login);
        }
    }
}

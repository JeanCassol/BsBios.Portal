using System;
using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using System.Linq;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class UsuariosTests: RepositoryTest
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

        /// <summary>
        /// nesse mesmo teste consigo testar os métodos Save e BuscaPorId
        /// </summary>
        [TestMethod]
        public void QuandoPersistoUmUsuarioComSucessoPossoConsultarOMesmoUsuario()
        {

            Usuario usuarioNovo = DefaultObjects.ObtemUsuarioPadrao();
            usuarioNovo.AdicionarPerfil(Enumeradores.Perfil.CompradorLogistica);
            DefaultPersistedObjects.PersistirUsuario(usuarioNovo);

            UnitOfWorkNh.Session.Clear();

            var usuarios = ObjectFactory.GetInstance<IUsuarios>();
            Usuario usuarioConsulta = usuarios.BuscaPorLogin(usuarioNovo.Login);

            Assert.IsNotNull(usuarioConsulta);
            Assert.AreEqual(usuarioNovo.Login, usuarioConsulta.Login);
            Assert.AreEqual(usuarioNovo.Nome, usuarioConsulta.Nome);
            Assert.IsNull(usuarioConsulta.Senha);
            Assert.AreEqual(usuarioNovo.Email, usuarioConsulta.Email);
            Assert.IsTrue(usuarioConsulta.Perfis.Contains(Enumeradores.Perfil.CompradorLogistica));
            Assert.AreEqual(Enumeradores.StatusUsuario.Ativo, usuarioConsulta.Status);

        }

        [TestMethod]
        public void ConsigoCadastrarUmUsuarioComTodosOsPerfis()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            var perfis = Enum.GetValues(typeof (Enumeradores.Perfil));
            foreach (var perfil in perfis)
            {
                usuario.AdicionarPerfil((Enumeradores.Perfil) perfil);
            }

            DefaultPersistedObjects.PersistirUsuario(usuario);

            UnitOfWorkNh.Session.Clear();

            var usuarios = ObjectFactory.GetInstance<IUsuarios>();
            Usuario usuarioConsulta = usuarios.BuscaPorLogin(usuario.Login);

            Assert.AreEqual(perfis.Length, usuarioConsulta.Perfis.Count);
            
        }

        [TestMethod]
        public void QuandoBuscarUsuarioPorLoginComLoginInexistenteDeveRetornarNull()
        {
            var usuarios = ObjectFactory.GetInstance<IUsuarios>();

            Usuario usuario = usuarios.BuscaPorLogin("inexistente");
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
                UnitOfWorkNh.Session.Save(usuarioNovo);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                    
                throw;
            }

            var usuarios = ObjectFactory.GetInstance<IUsuarios>();

            Usuario usuario = usuarios.BuscaPorLogin(usuarioNovo.Login);
            Assert.IsNotNull(usuario);
            Assert.AreEqual(usuarioNovo.Login,usuario.Login);
        }

        [TestMethod]
        public void QuandoFiltroPorListaDeLoginTemQueRetornarUsuariosCorrespondenteAosLogins()
        {
            UnitOfWorkNh.BeginTransaction();
            Usuario usuario1 = DefaultObjects.ObtemUsuarioPadrao();
            Usuario usuario2 = DefaultObjects.ObtemUsuarioPadrao();
            Usuario usuario3 = DefaultObjects.ObtemUsuarioPadrao();
            UnitOfWorkNh.Session.Save(usuario1);
            UnitOfWorkNh.Session.Save(usuario2);
            UnitOfWorkNh.Session.Save(usuario3);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var usuarios = ObjectFactory.GetInstance<IUsuarios>();
            IList<Usuario> usuariosConsultados = usuarios.FiltraPorListaDeLogins(new[] { usuario1.Login, usuario2.Login }).List();

            Assert.AreEqual(2, usuariosConsultados.Count);
            Assert.AreEqual(1, usuariosConsultados.Count(x => x.Login == usuario1.Login));
            Assert.AreEqual(1, usuariosConsultados.Count(x => x.Login == usuario2.Login));

        }


    }
}

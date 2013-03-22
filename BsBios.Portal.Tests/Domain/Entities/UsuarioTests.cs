using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class UsuarioTests
    {
        [TestMethod]
        public void QuandoCrioUmUsuarioAsPropridadesFicamCorretas()
        {
            var usuario = new Usuario("Mauro Leal", "mauroscl", "mauro.leal@fusionconsultoria.com.br");
            Assert.AreEqual("Mauro Leal", usuario.Nome);
            Assert.AreEqual("mauroscl", usuario.Login);
            Assert.IsNull(usuario.Senha);
            Assert.AreEqual("mauro.leal@fusionconsultoria.com.br",usuario.Email);
            Assert.AreEqual(0,usuario.Perfis.Count);
            Assert.AreEqual(Enumeradores.StatusUsuario.Ativo,usuario.Status);
        }

        [TestMethod]
        public void QuandoCriaUmaSenhaEstaEAlterada()
        {
            var usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.CriarSenha("vetoa1051067");
            Assert.AreEqual("vetoa1051067", usuario.Senha);
        }

        [TestMethod]
        public void QuandoAdicionoUmPerfilOUsuarioPassaAContarEstePerfil()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();            
            usuario.AdicionarPerfil(Enumeradores.Perfil.CompradorLogistica);
            Assert.AreEqual(1, usuario.Perfis.Count(x => x == Enumeradores.Perfil.CompradorLogistica));
        }


        [TestMethod]
        public void QuandoRemovoUmPerfilOUsuarioNaoCotemMaisEstePerfil()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.CompradorLogistica);
            usuario.RemoverPerfil(Enumeradores.Perfil.CompradorLogistica);
            Assert.AreEqual(0, usuario.Perfis.Count(x => x == Enumeradores.Perfil.CompradorLogistica));
        }

        [TestMethod]
        public void QuandoBloqueioUsuarioFicaComStatusBloqueado()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.Bloquear();
            Assert.AreEqual(Enumeradores.StatusUsuario.Bloqueado, usuario.Status);
        }

        [TestMethod]
        public void QuandoAtivoUsuarioFicaComEstadoAtivo()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.Bloquear();
            usuario.Ativar();
            Assert.AreEqual(Enumeradores.StatusUsuario.Ativo, usuario.Status);
        }
        [TestMethod]
        public void QuandoAlterarSenhaUsuarioUsuarioDeveTerSenhaAlterada()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.CriarSenha("123");
            usuario.AlterarSenha("123","456");
            Assert.AreEqual("456", usuario.Senha);
        }
        [TestMethod]
        [ExpectedException(typeof(SenhaIncorretaException))]
        public void QuandoAlterarSenhaInformandoSenhaAtualIncorretaDeveGerarExcecao()
        {
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.CriarSenha("123");
            usuario.AlterarSenha("1234", "456");
            Assert.AreEqual("456", usuario.Senha);
        }

    }
}

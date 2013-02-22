using System;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class UsuarioTests
    {
        [TestMethod]
        public void QuandoCrioUmUsuarioAsPropridadesFicamCorretas()
        {
            var usuario = new Usuario("Mauro Leal", "mauroscl", "mauro.leal@fusionconsultoria.com.br", Enumeradores.Perfil.Comprador);
            Assert.AreEqual("Mauro Leal", usuario.Nome);
            Assert.AreEqual("mauroscl", usuario.Login);
            Assert.IsNull(usuario.Senha);
            Assert.AreEqual("mauro.leal@fusionconsultoria.com.br",usuario.Email);
            Assert.AreEqual(Enumeradores.Perfil.Comprador, usuario.Perfil);
        }

        [TestMethod]
        public void QuandoCriaUmaSenhaEstaEAlterada()
        {
            var usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.CriarSenha("vetoa1051067");
            Assert.AreEqual("vetoa1051067", usuario.Senha);
        }

    }
}

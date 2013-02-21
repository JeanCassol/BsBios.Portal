using System;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class UsuarioTests
    {
        [TestMethod]
        public void QuandoCrioConsigoAcessarTodasAsPropridades()
        {
            var usuario = new Usuario("Mauro Leal", "mauroscl", "123", "mauro.leal@fusionconsultoria.com.br", Enumeradores.Perfil.Comprador);
            Assert.AreEqual("Mauro Leal", usuario.Nome);
            Assert.AreEqual("mauroscl", usuario.Login);
            Assert.AreEqual("123",usuario.Senha);
            Assert.AreEqual("mauro.leal@fusionconsultoria.com.br",usuario.Email);
            Assert.AreEqual(Enumeradores.Perfil.Comprador, usuario.Perfil);
        }
        
    }
}

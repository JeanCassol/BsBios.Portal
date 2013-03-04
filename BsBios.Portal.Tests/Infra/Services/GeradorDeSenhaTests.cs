using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class GeradorDeSenhaTests
    {
        [TestMethod]
        public void SenhaGeradaTemONumeroDeCaracteresDesejados()
        {
            var geradorDeSenha = new GeradorDeSenha();
            var senha = geradorDeSenha.GerarGuid(10);
            Assert.AreEqual(10, senha.Length);
        }


        [TestMethod]
        public void QuandoGeroMilSenhasTodasSaoDiferentes()
        {
            var geradorDeSenha = new GeradorDeSenha();
            IList<string> senhasGeradas = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                var senha = geradorDeSenha.GerarGuid(8);
                Console.WriteLine(senha);
                Assert.IsFalse(senhasGeradas.Contains(senha));
                senhasGeradas.Add(senha);
            }
        }
    }
}

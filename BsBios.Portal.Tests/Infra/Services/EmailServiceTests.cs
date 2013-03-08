using System;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public void ConsigoEnviarEmailUtilizandoGmail()
        {
            var contaDeEmail = new ContaDeEmail("compras@bsbios.com", "", "mauroscl", "@#20mscl10@#", "smtp.gmail.com");
            var emailService = new EmailService(contaDeEmail);
            emailService.AdicionarDestinario("mauro.leal@fusionconsultoria.com.br");
            var mensagemDeEmail = new MensagemDeEmail("Teste Portal " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                "Mensagem de Teste enviada pelo portal de Cotações da Bs Bios");
            Assert.IsTrue(emailService.Enviar(mensagemDeEmail));
        }

        [TestMethod]
        public void ConsigoEnviarEmailUtilizandoContaDaBsBios()
        {
            var contaDeEmail = new ContaDeEmail("compras@bsbios.com","bsbios.com", "sistemas", "B5@dm99", "mail.bsbios.com");
            var emailService = new EmailService(contaDeEmail);
            emailService.AdicionarDestinario("mauro.leal@fusionconsultoria.com.br");
            var mensagemDeEmail = new MensagemDeEmail("Teste Portal " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                "Mensagem de Teste enviada pelo portal de Cotações da Bs Bios");
            Assert.IsTrue(emailService.Enviar(mensagemDeEmail));
            
        }
    }
}

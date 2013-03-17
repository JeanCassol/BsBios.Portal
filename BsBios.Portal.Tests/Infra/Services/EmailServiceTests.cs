using System;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Services
{
    //Removido anotação "TestClass" para não ficar mandando e-mail sempre que rodar os testes
    //[TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public void ConsigoEnviarEmailUtilizandoGmail()
        {
            var contaDeEmail = new ContaDeEmail("mauroscl@gmail.com", "", "mauroscl", "@#20mscl10@#", "smtp.gmail.com",587);
            var emailService = new EmailService(contaDeEmail);
            emailService.AdicionarDestinatario("mauro.leal@fusionconsultoria.com.br");
            var mensagemDeEmail = new MensagemDeEmail("Teste Portal " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                "Mensagem de Teste enviada pelo portal de Cotações da Bs Bios");
            Assert.IsTrue(emailService.Enviar(mensagemDeEmail));
        }

        [TestMethod]
        public void ConsigoEnviarEmailUtilizandoContaDaBsBios()
        {
            var contaDeEmail = new ContaDeEmail("compras@bsbios.com","bsbios.com", "sistemas", "B5@dm99", "mail.bsbios.com",25);
            var emailService = new EmailService(contaDeEmail);
            emailService.AdicionarDestinatario("mauro.leal@fusionconsultoria.com.br");
            var mensagemDeEmail = new MensagemDeEmail("Teste Portal " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                "Mensagem de Teste enviada pelo portal de Cotações da Bs Bios");
            Assert.IsTrue(emailService.Enviar(mensagemDeEmail));
            
        }

        [TestMethod]
        public void ConsigoEnviarEmailUtilizandoAsConfiguracoesDoArquivoDeConfiguracao()
        {
            var emailService = ObjectFactory.GetInstance<IEmailService>();
            emailService.AdicionarDestinatario("mauro.leal@fusionconsultoria.com.br");
            var mensagemDeEmail = new MensagemDeEmail("Teste Portal " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                "Mensagem de Teste enviada pelo portal de Cotações da Bs Bios");
            Assert.IsTrue(emailService.Enviar(mensagemDeEmail));

        }
    }
}

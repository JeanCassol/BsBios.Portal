using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ContaDeEmail _contaDeEmail;
        private readonly IList<string> _destinatarios;

        public EmailService(ContaDeEmail contaDeEmail)
        {
            _contaDeEmail = contaDeEmail;
            _destinatarios = new List<string>();
        }

        public void AdicionarDestinario(string destinatario)
        {
            _destinatarios.Add(destinatario);
        }

        public bool Enviar(MensagemDeEmail mensagemDeEmail)
        {
            if (!_destinatarios.Any())
            {
                throw new Exception("Não existem destinatários para enviar o e-mail");
            }
            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient(_contaDeEmail.ServidorSmtp);

            mailMessage.From = new MailAddress(_contaDeEmail.EmailDoRemetente);
            foreach (var destinatario in _destinatarios)
            {
                mailMessage.To.Add(destinatario);    
            }
            
            mailMessage.Subject = mensagemDeEmail.Assunto;

            mailMessage.Body = mensagemDeEmail.Conteudo;

            //smtpClient.Port = 587;
            //smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(_contaDeEmail.Usuario, _contaDeEmail.Senha, _contaDeEmail.Dominio);
            //smtpClient.Credentials = new NetworkCredential(_contaDeEmail.Usuario, _contaDeEmail.Senha);
            //smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
            return true;
        }
    }
}

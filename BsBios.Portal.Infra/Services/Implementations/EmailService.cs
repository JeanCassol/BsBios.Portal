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

        public void AdicionarDestinatario(string destinatario)
        {
            _destinatarios.Add(destinatario);
        }

        public bool Enviar(MensagemDeEmail mensagemDeEmail)
        {
            if (!_destinatarios.Any())
            {
                throw new Exception("Não existem destinatários para enviar o e-mail");
            }
            var smtpClient = new SmtpClient(_contaDeEmail.ServidorSmtp)
                {
                    Port = _contaDeEmail.Porta,
                    Credentials = new NetworkCredential(_contaDeEmail.Usuario, _contaDeEmail.Senha, _contaDeEmail.Dominio),
                    EnableSsl = _contaDeEmail.HabilitarSsl
                };

            //smtpClient.UseDefaultCredentials = true;
            //smtpClient.Credentials = new NetworkCredential(_contaDeEmail.Usuario, _contaDeEmail.Senha);

            var mailMessage = new MailMessage {From = new MailAddress(_contaDeEmail.EmailDoRemetente)};

            foreach (var destinatario in _destinatarios)
            {
                mailMessage.To.Add(destinatario);    
            }
            
            mailMessage.Subject = mensagemDeEmail.Assunto;

            mailMessage.Body = mensagemDeEmail.Conteudo;


            smtpClient.Send(mailMessage);
            return true;
        }

        public bool Enviar(string destinatario, MensagemDeEmail mensagemDeEmail)
        {
            _destinatarios.Clear();
            _destinatarios.Add(destinatario);
            Enviar(mensagemDeEmail);
            return true;
        }
    }
}

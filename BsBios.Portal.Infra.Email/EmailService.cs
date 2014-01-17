using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BsBios.Portal.Infra.Email
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

        private bool EmailEValido(string email)
        {
            var rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            return rg.IsMatch(email);
        }

        public void AdicionarDestinatario(string destinatario)
        {
            if (string.IsNullOrEmpty(destinatario))
            {
                throw new Exception("E-mail não preenchido. Não é possível enviar e-mail.");
            }
            if (!EmailEValido(destinatario))
            {
                throw new Exception("O e-mail \"" + destinatario + "\" não é válido.");
            }
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

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Ocorreu um erro ao enviar e-mail.",ex);
            }
            
            return true;
        }

        public bool Enviar(string destinatario, MensagemDeEmail mensagemDeEmail)
        {
            _destinatarios.Clear();
            AdicionarDestinatario(destinatario);
            Enviar(mensagemDeEmail);
            return true;
        }
    }
}

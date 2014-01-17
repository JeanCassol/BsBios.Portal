namespace BsBios.Portal.Infra.Email
{
    public interface IEmailService
    {
        bool Enviar(MensagemDeEmail mensagemDeEmail);
        bool Enviar(string destinatario, MensagemDeEmail mensagemDeEmail);
        void AdicionarDestinatario(string destinatario);
    }

}
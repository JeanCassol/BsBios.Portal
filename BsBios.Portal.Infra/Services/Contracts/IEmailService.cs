using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IEmailService
    {
        bool Enviar(MensagemDeEmail mensagemDeEmail);
        bool Enviar(string destinatario, MensagemDeEmail mensagemDeEmail);
        void AdicionarDestinatario(string destinatario);
    }
}

using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Email;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeEmail : IGeradorDeEmail
    {
        //private readonly ContaDeEmail _contaDeEmail ;
        private readonly GeradorDeMensagemDeEmail _geradorDeMensagemDeEmail;

        public GeradorDeEmail(GeradorDeMensagemDeEmail geradorDeMensagemDeEmail)
        {
            _geradorDeMensagemDeEmail = geradorDeMensagemDeEmail;
        }

        public void CriacaoAutomaticaDeSenha(Usuario usuario, string novaSenha)
        {
            if (string.IsNullOrEmpty(usuario.Email))
            {
                throw new UsuarioSemEmailException(usuario.Nome);
            }
            var contaDeEmail = ObjectFactory.GetNamedInstance<ContaDeEmail>("ContaDeEmailDaLogistica");
            var emailService = new EmailService(contaDeEmail);
            emailService.Enviar(usuario.Email, _geradorDeMensagemDeEmail.CriacaoAutomaticaDeSenha(usuario, novaSenha));
        }

    }
}
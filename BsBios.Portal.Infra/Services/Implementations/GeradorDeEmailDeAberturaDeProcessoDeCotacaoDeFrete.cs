using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFrete : IGeradorDeEmailDeAberturaDeProcessoDeCotacao
    {
        private readonly IGeradorDeMensagemDeEmail _geradorDeMensagemDeEmail;
        private readonly IEmailService _emailService;
        public GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFrete(IGeradorDeMensagemDeEmail geradorDeMensagemDeEmail, 
            IEmailService emailService )
        {
            _geradorDeMensagemDeEmail = geradorDeMensagemDeEmail;
            _emailService = emailService;
        }

        public void GerarEmail(ProcessoDeCotacao processoDeCotacao)
        {
            MensagemDeEmail mensagemDeEmail = _geradorDeMensagemDeEmail.AberturaDoProcessoDeCotacaoDeFrete(processoDeCotacao);
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
            }
            
        }

        public void GerarEmail(FornecedorParticipante fornecedorParticipante)
        {
            MensagemDeEmail mensagemDeEmail = _geradorDeMensagemDeEmail.AberturaDoProcessoDeCotacaoDeFrete(fornecedorParticipante.ProcessoDeCotacao);
            _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
        }
    }
}
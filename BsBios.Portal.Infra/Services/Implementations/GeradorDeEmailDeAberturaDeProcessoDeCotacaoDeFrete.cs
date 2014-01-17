using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Email;
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
            var processoDeCotacaoDeFrete = (ProcessoDeCotacaoDeFrete) processoDeCotacao;
            MensagemDeEmail mensagemDeEmail = _geradorDeMensagemDeEmail.AberturaDoProcessoDeCotacaoDeFrete(processoDeCotacaoDeFrete);
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
            }
            
        }

        public void GerarEmail(FornecedorParticipante fornecedorParticipante)
        {
            var processoDeCotacaoDeFrete = (ProcessoDeCotacaoDeFrete) fornecedorParticipante.ProcessoDeCotacao;
            MensagemDeEmail mensagemDeEmail = _geradorDeMensagemDeEmail.AberturaDoProcessoDeCotacaoDeFrete(processoDeCotacaoDeFrete);
            _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
        }
    }
}
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeEmailDeFechamentoDeProcessoDeCotacao : IGeradorDeEmailDeFechamentoDeProcessoDeCotacao
    {
        private readonly IGeradorDeMensagemDeEmail _geradorDeMensagemDeEmail;
        private readonly IEmailService _emailService;
        public GeradorDeEmailDeFechamentoDeProcessoDeCotacao(IGeradorDeMensagemDeEmail geradorDeMensagemDeEmail, 
            IEmailService emailService)
        {
            _geradorDeMensagemDeEmail = geradorDeMensagemDeEmail;
            _emailService = emailService;
        }

        public void GerarEmail(ProcessoDeCotacao processoDeCotacao)
        {
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacao = fornecedorParticipante.Cotacao;
                if (cotacao == null) continue;
                MensagemDeEmail mensagemDeEmail = cotacao.Selecionada ? 
                                                      _geradorDeMensagemDeEmail.FornecedoresSelecionadosNoProcessoDeCotacao(processoDeCotacao, cotacao) : 
                                                      _geradorDeMensagemDeEmail.FornecedoresNaoSelecionadosNoProcessoDeCotacao(cotacao);

                _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
            }
            
        }
    }
}
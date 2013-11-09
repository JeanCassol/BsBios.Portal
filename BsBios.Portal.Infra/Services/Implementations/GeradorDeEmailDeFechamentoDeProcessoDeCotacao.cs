using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeEmailDeFechamentoDeProcessoDeCotacao : IGeradorDeEmailDeFechamentoDeProcessoDeCotacao
    {
        protected readonly IGeradorDeMensagemDeEmail GeradorDeMensagemDeEmail;
        protected readonly IEmailService EmailService;
        public GeradorDeEmailDeFechamentoDeProcessoDeCotacao(IGeradorDeMensagemDeEmail geradorDeMensagemDeEmail, 
            IEmailService emailService)
        {
            GeradorDeMensagemDeEmail = geradorDeMensagemDeEmail;
            EmailService = emailService;
        }

        public virtual void GerarEmail(ProcessoDeCotacao processoDeCotacao)
        {
 
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacao = fornecedorParticipante.Cotacao;
                if (cotacao == null) continue;
                MensagemDeEmail mensagemDeEmail = cotacao.Selecionada ? 
                                                      GeradorDeMensagemDeEmail.FornecedoresSelecionadosNoProcessoDeCotacao(processoDeCotacao, cotacao) : 
                                                      GeradorDeMensagemDeEmail.FornecedoresNaoSelecionadosNoProcessoDeCotacao(cotacao);

                EmailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
            }
            
        }
    }

    public class GeradorDeEmailDeFechamentoDeProcessoDeCotacaoDeFrete:GeradorDeEmailDeFechamentoDeProcessoDeCotacao
    {
        public GeradorDeEmailDeFechamentoDeProcessoDeCotacaoDeFrete(IGeradorDeMensagemDeEmail geradorDeMensagemDeEmail, IEmailService emailService) 
            : base(geradorDeMensagemDeEmail, emailService)
        {

        }

        public override void GerarEmail(ProcessoDeCotacao processoDeCotacao)
        {
            var processoDeCotacaoDeFrete = (ProcessoDeCotacaoDeFrete) processoDeCotacao;
            Fornecedor fornecedorDaMercadoria = processoDeCotacaoDeFrete.FornecedorDaMercadoria;
            if (fornecedorDaMercadoria != null)
            {
                MensagemDeEmail mensagemDeEmail = GeradorDeMensagemDeEmail.AutorizacaoDeTransporte(processoDeCotacaoDeFrete);
                EmailService.Enviar(fornecedorDaMercadoria.Email, mensagemDeEmail);
            }
            base.GerarEmail(processoDeCotacao);
        }
    }
}
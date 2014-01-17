using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Email;
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

        public virtual void GerarEmail(ProcessoDeCotacao processoDeCotacao)
        {
 
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacao = fornecedorParticipante.Cotacao;
                if (cotacao == null) continue;
                MensagemDeEmail mensagemDeEmail = cotacao.Selecionada ? 
                                                      _geradorDeMensagemDeEmail.FornecedoresSelecionadosNoProcessoDeCotacao(processoDeCotacao, cotacao) :
                                                      _geradorDeMensagemDeEmail.FornecedoresNaoSelecionadosNoProcessoDeCotacao(processoDeCotacao);

                _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
            }
            
        }
    }

    public class GeradorDeEmailDeFechamentoDeProcessoDeCotacaoDeFrete : IGeradorDeEmailDeFechamentoDeProcessoDeCotacao
    {
        private readonly IGeradorDeMensagemDeEmail _geradorDeMensagemDeEmail;
        private readonly IEmailService _emailService;

        public GeradorDeEmailDeFechamentoDeProcessoDeCotacaoDeFrete(IGeradorDeMensagemDeEmail geradorDeMensagemDeEmail, IEmailService emailService)
        {
            _geradorDeMensagemDeEmail = geradorDeMensagemDeEmail;
            _emailService = emailService;
        }

        public void GerarEmail(ProcessoDeCotacao processoDeCotacao)
        {
            var processoDeCotacaoDeFrete = (ProcessoDeCotacaoDeFrete) processoDeCotacao;
            Fornecedor fornecedorDaMercadoria = processoDeCotacaoDeFrete.FornecedorDaMercadoria;
            if (fornecedorDaMercadoria != null)
            {
                if (string.IsNullOrEmpty(fornecedorDaMercadoria.Email))
                {
                    throw new Exception("O e-mail do fornecedor " + fornecedorDaMercadoria.Nome + " não está preenchido.");
                }
                MensagemDeEmail mensagemDeEmail = _geradorDeMensagemDeEmail.AutorizacaoDeTransporte(processoDeCotacaoDeFrete);
                _emailService.Enviar(fornecedorDaMercadoria.Email, mensagemDeEmail);
            }
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacao = fornecedorParticipante.Cotacao;
                if (cotacao == null) continue;
                MensagemDeEmail mensagemDeEmail = cotacao.Selecionada ?
                                                      _geradorDeMensagemDeEmail.FornecedoresSelecionadosNoProcessoDeCotacaoDeFrete(processoDeCotacaoDeFrete, cotacao) :
                                                      _geradorDeMensagemDeEmail.FornecedoresNaoSelecionadosNoProcessoDeCotacao(processoDeCotacaoDeFrete);

                _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
            }
        }
    }
}
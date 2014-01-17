using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Email;
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IGeradorDeMensagemDeEmail
    {
        MensagemDeEmail CriacaoAutomaticaDeSenha(Usuario usuario, string novaSenha);
        MensagemDeEmail AberturaDoProcessoDeCotacaoDeFrete(ProcessoDeCotacaoDeFrete processoDeCotacao);
        MensagemDeEmail AberturaDoProcessoDeCotacaoDeMaterial(ProcessoDeCotacao processoDeCotacao);
        MensagemDeEmail FornecedoresSelecionadosNoProcessoDeCotacao(ProcessoDeCotacao processoDeCotacao, Cotacao cotacao);
        MensagemDeEmail FornecedoresSelecionadosNoProcessoDeCotacaoDeFrete(ProcessoDeCotacaoDeFrete processoDeCotacao, Cotacao cotacao);
        MensagemDeEmail FornecedoresNaoSelecionadosNoProcessoDeCotacao(ProcessoDeCotacao processoDeCotacao);
        MensagemDeEmail AutorizacaoDeTransporte(ProcessoDeCotacaoDeFrete processoDeCotacao);
    }
}
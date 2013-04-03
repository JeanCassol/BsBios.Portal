using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IGeradorDeMensagemDeEmail
    {
        MensagemDeEmail CriacaoAutomaticaDeSenha(Usuario usuario, string novaSenha);
        MensagemDeEmail AberturaDoProcessoDeCotacaoDeFrete(ProcessoDeCotacao processoDeCotacao);
        MensagemDeEmail AberturaDoProcessoDeCotacaoDeMaterial(ProcessoDeCotacao processoDeCotacao);
        MensagemDeEmail FornecedoresSelecionadosNoProcessoDeCotacao(ProcessoDeCotacao processoDeCotacao, Cotacao cotacao);
        MensagemDeEmail FornecedoresNaoSelecionadosNoProcessoDeCotacao(Cotacao cotacao);
    }
}
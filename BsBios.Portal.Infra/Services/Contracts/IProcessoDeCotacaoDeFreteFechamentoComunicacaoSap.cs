using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IProcessoDeCotacaoDeFreteFechamentoComunicacaoSap
    {
        ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacaoDeFrete processo);
    }
}
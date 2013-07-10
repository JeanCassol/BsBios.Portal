using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IProcessoDeCotacaoComunicacaoSap
    {
        ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo);
    }
}
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IComunicacaoSap
    {
        ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo);
    }
}
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IGeradorDeEmailDeFechamentoDeProcessoDeCotacao
    {
        void GerarEmail(ProcessoDeCotacao processoDeCotacao);
    }
}
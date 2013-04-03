using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IGeradorDeEmailDeAberturaDeProcessoDeCotacao
    {
        void GerarEmail(ProcessoDeCotacao processoDeCotacao);
    }
}
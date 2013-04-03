using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IGeradorDeEmail
    {
        void CriacaoAutomaticaDeSenha(Usuario usuario, string novaSenha);
    }
}
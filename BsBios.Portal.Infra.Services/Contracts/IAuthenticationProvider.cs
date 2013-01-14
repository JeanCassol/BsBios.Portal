using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IAuthenticationProvider
    {
        UsuarioConectado Autenticar(string usuario, string senha);
    }
}
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IAuthenticationProvider
    {
        void Autenticar(UsuarioConectado usuarioConectado);
    }
}
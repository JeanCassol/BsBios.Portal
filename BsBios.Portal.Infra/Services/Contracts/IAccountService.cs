using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IAccountService
    {
        UsuarioConectado Login(string usuario, string senha);
        void Logout();
    }
}
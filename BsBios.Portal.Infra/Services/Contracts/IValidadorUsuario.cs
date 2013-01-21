using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IValidadorUsuario
    {
        UsuarioConectado Validar(string usuario, string senha); 
    }
}
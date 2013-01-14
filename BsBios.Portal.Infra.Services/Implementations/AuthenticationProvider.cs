using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class AuthenticationProvider: IAuthenticationProvider
    {
        public UsuarioConectado Autenticar(string usuario, string senha)
        {
            usuario = usuario.ToLower();
            if (usuario == "comprador" && senha == "123")
            {
                return new UsuarioConectado(usuario,new PerfilComprador());
            }
            if (usuario == "fornecedor" && senha == "123")
            {
                return new UsuarioConectado(usuario, new PerfilFornecedor());
            }

            return new UsuarioConectado(usuario,new PerfilNaoAutorizado());
        }
    }
}

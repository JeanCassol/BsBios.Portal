using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class AccountService: IAccountService
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly IValidadorUsuario _validadorUsuario;

        public AccountService(IAuthenticationProvider authenticationProvider, IValidadorUsuario validadorUsuario)
        {
            _authenticationProvider = authenticationProvider;
            _validadorUsuario = validadorUsuario;
        }

        public UsuarioConectado Login(string usuario, string senha)
        {
            UsuarioConectado usuarioConectado = _validadorUsuario.Validar(usuario, senha);
            _authenticationProvider.Autenticar( usuarioConectado);
            return usuarioConectado;
        }

        public void Logout()
        {
            _authenticationProvider.Desconectar();
        }
    }
}

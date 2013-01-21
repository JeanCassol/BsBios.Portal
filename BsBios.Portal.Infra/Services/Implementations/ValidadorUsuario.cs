using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ValidadorUsuario: IValidadorUsuario
    {
        public UsuarioConectado Validar(string usuario, string senha)
        {
            usuario = usuario.ToLower();
            UsuarioConectado usuarioConectado;
            if (usuario == "comprador" && senha == "123")
            {
                usuarioConectado = new UsuarioConectado(usuario, new PerfilComprador());
            }
            else if (usuario == "fornecedor" && senha == "123")
            {
                usuarioConectado = new UsuarioConectado(usuario, new PerfilFornecedor());
            }
            else
            {
                usuarioConectado = new UsuarioConectado(usuario, new PerfilNaoAutorizado());
            }

            return usuarioConectado;
        }
    }
}

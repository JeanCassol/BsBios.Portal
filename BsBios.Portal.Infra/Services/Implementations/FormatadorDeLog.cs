using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class FormatadorDeLog : IFormatadorDeLog
    {
        private readonly UsuarioConectado _usuarioConectado;

        public FormatadorDeLog(UsuarioConectado usuarioConectado)
        {
            _usuarioConectado = usuarioConectado;
        }
        public string FormatarUsuario()
        {
            return $"[{_usuarioConectado.Login} - {_usuarioConectado.NomeCompleto}]";

        }
    }
}
using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class UsuarioBloqueadoException: Exception
    {
        private readonly string _login;

        public UsuarioBloqueadoException(string login)
        {
            _login = login;
        }

        public override string Message
        {
            get { return "O usuário "  + _login + " está bloqueado.  Entre em contato com o suporte"; }
        }
    }
}

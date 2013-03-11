using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class UsuarioSemPerfilException: Exception
    {
        private readonly string _login;

        public UsuarioSemPerfilException(string login)
        {
            _login = login;
        }

        public override string Message
        {
            get { return "O usuário "  + _login + " não possui um perfil associado.  Entre em contato com o suporte"; }
        }
    }
}

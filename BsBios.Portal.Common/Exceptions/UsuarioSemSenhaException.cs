using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class UsuarioSemSenhaException: Exception
    {
        private readonly string _login;

        public UsuarioSemSenhaException(string login)
        {
            _login = login;
        }

        public override string Message
        {
            get { return "O usuário " + _login + " não possui senha. Entre em contato com o suporte."; }
        }
    }
}

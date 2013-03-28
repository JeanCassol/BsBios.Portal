using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class UsuarioSemEmailException: Exception
    {
        private readonly string _nomeDoUsuario;
        public UsuarioSemEmailException(string nomeDoUsuario )
        {
            _nomeDoUsuario = nomeDoUsuario;
        }

        public override string Message
        {
            get { return "Não é possível enviar e-mail para o usuário " + _nomeDoUsuario + " porque este usuário não possui um e-mail cadastrado."; }
        }
    }
}

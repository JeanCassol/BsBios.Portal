using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Common.Exceptions
{
    public class UsuarioNaoCadastradoException : Exception
    {
        private readonly string _nomeDoUsuario;

        public UsuarioNaoCadastradoException(string nomeDoUsuario)
        {
            _nomeDoUsuario = nomeDoUsuario;
        }

        public override string Message
        {
            get { return "O usuário " + _nomeDoUsuario  + " não está cadastrado."; }
        }
    }
}

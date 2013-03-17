using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ComunicacaoSapException: Exception
    {
        private readonly string _mensagem;

        public ComunicacaoSapException(string mensagem)
        {
            _mensagem = mensagem;
        }

        public override string Message
        {
            get { return _mensagem; }
        }
    }
}

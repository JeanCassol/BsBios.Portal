using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ComunicacaoSapException: Exception
    {
        public string MediaType { get; protected set; }
        private readonly string _mensagem;

        public ComunicacaoSapException(string mediaType, string mensagem)
        {
            MediaType = mediaType;
            _mensagem = mensagem;
        }

        public override string Message
        {
            get { return _mensagem; }
        }
    }
}

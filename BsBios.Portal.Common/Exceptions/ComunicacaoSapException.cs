using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ComunicacaoSapException: Exception
    {
        private readonly string _mensagem;
        public string MediaType { get; protected set; }       

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

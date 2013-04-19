using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class FecharProcessoDeCotacaoFechadoException : Exception
    {
        public override string Message
        {
            get { return "  Não é possível fechar um Processo de Cotação que já está fechado."; }
        }
    }
}
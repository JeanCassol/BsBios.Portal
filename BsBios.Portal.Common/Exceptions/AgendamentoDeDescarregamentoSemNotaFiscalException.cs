using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class AgendamentoDeDescarregamentoSemNotaFiscalException: Exception
    {
        public override string Message
        {
            get { return "É necessário informar pelo menos uma nota fiscal."; }
        }
    }
}

using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ColetaSemNotaFiscalException: Exception
    {
        public override string Message
        {
            get { return "É necessário informar pelo menos uma Nota Fiscal na Coleta."; }
        }
    }
}

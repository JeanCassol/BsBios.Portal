using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class DataDeValidadeInicialMaiorQueDataDeValidadeFinalException: Exception
    {
        public override string Message
        {
            get { return "A Data de Validade Final deve ser maior que a Data de Validade Inicial."; }
        }
    }
}

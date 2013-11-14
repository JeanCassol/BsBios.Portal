using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class RealizacaoDeColetaRealizadaException : Exception
    {
        public override string Message
        {
            get { return "Esta coleta já foi realizada. Não é permitido realizar novamente"; }
        }
    }
}

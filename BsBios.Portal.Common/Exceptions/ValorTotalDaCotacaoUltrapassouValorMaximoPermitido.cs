using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ValorTotalDaCotacaoUltrapassouValorMaximoPermitido : Exception
    {
        public override string Message
        {
            get { return "Valor com impostos ultrapassou o valor máximo permitido no processo de cotação."; }
        }
    }
}
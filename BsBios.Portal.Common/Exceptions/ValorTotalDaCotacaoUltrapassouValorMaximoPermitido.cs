using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ValorTotalDaCotacaoUltrapassouValorMaximoPermitido : Exception
    {
        public override string Message
        {
            get { return "Valor com impostos ultrapassou o valor m�ximo permitido no processo de cota��o."; }
        }
    }
}
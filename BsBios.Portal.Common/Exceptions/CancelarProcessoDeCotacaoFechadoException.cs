using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class CancelarProcessoDeCotacaoFechadoException: Exception
    {
        public override string Message
        {
            get { return "Não é permitido cancelar um processo de cotação que está fechado."; }
        }
    }
}
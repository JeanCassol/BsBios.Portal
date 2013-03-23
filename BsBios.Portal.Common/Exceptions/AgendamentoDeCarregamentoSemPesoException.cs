using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class AgendamentoDeCarregamentoSemPesoException: Exception
    {
        public override string Message
        {
            get { return "É necessário informar um peso para fazer o agendamento."; }
        }
    }
}

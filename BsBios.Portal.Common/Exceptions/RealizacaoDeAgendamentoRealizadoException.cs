using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class RealizacaoDeAgendamentoRealizadoException: Exception
    {
        public override string Message
        {
            get { return "Este agendamento já foi realizado. Não é permitido realizar novamente"; }
        }
    }
}

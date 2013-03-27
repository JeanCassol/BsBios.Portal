using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class DataDeAgendamentoExpiradaException : Exception
    {
        private readonly string _mensagem;
        public DataDeAgendamentoExpiradaException(string mensagem)
        {
            _mensagem = mensagem;
        }

        public override string Message
        {
            get { return _mensagem; }
        }
    }
}
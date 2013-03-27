using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class OperacaoNaoPermitidaParaAgendamentoRealizadoException : Exception
    {
        private readonly string _mensagem;
        public OperacaoNaoPermitidaParaAgendamentoRealizadoException(string mensagem)
        {
            _mensagem = mensagem;
        }
        public override string Message
        {
            get { return _mensagem; }
        }

    }
}
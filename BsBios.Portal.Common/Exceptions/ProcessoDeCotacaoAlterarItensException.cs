using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoAlterarItensException: Exception
    {
        private readonly string _status;

        public ProcessoDeCotacaoAlterarItensException(string status)
        {
            _status = status;
        }

        public override string Message
        {
            get { return String.Format("Não é permitido alterar itens de um Processo de Cotação que está com Status {0}.",_status); }
        }
    }
}
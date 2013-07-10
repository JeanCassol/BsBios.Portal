using System;
namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoAtualizacaoDadosException: Exception
    {
        private readonly string _statusAtual;

        public ProcessoDeCotacaoAtualizacaoDadosException(string statusAtual)
        {
            _statusAtual = statusAtual;
        }

        public override string Message
        {
            get
            {
                return "Não é possível atualizar um Processo de Cotação que está com Status " + _statusAtual + ".";
            }
        }
    }
}

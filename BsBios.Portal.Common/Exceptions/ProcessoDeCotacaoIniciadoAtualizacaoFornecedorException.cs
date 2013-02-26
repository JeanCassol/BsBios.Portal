using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException: Exception
    {
        private readonly string _statusAtual;

        public ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException(string statusAtual)
        {
            _statusAtual = statusAtual;
        }

        public override string Message
        {
            get
            {
                return "Não é possível atualizar os Fornecedores de um Processo de Cotação que está com Status " + _statusAtual + ".";
            }
        }
    }
}

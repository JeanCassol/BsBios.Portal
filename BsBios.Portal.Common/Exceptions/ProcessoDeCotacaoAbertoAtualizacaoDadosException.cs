using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoAbertoAtualizacaoDadosException: Exception
    {
        private readonly string _statusAtual;

        public ProcessoDeCotacaoAbertoAtualizacaoDadosException(string statusAtual)
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

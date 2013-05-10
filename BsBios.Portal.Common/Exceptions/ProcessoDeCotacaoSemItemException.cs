using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoSemItemException: Exception
    {
        public override string Message
        {
            get { return "Não é possível abrir um Processo de Cotação sem adicionar pelo menos um item."; }
        }
    }
}

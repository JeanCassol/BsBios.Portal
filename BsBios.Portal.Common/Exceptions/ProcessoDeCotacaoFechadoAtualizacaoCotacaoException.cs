using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoFechadoAtualizacaoCotacaoException: Exception
    {
        public override string Message
        {
            get
            {
                return "Não é possível atualizar a cotação de um Processo de Cotação com Status Fechado.";
            }
        }
    }
}

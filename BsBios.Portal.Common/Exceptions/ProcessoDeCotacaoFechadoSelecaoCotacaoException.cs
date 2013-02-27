using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoFechadoSelecaoCotacaoException : Exception
    {
        public override string Message
        {
            get
            {
                return "Não é possível selecionar uma cotação de um Processo de Cotação com Status Fechado.";
            }
        }
    }
}

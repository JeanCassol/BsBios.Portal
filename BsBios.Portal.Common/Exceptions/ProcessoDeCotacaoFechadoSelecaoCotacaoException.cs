using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoFechadoSelecaoCotacaoException : Exception
    {
        public override string Message
        {
            get
            {
                return "Não é possível alterar as cotações selecionadas em um Processo de Cotação com Status Fechado.";
            }
        }
    }
}

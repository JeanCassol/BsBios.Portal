using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoFecharSemCotacaoSelecionadaException: Exception
    {
        public override string Message
        {
            get
            {
                return "Não é possível fechar um Processo de Cotação sem selecionar pela menos um Fornecedor.";
            }
        }
    }
}

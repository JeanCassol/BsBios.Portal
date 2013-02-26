using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoSemFornecedoresException: Exception
    {
        public override string Message
        {
            get
            {
                return "Não é possível iniciar o Processo de Cotação antes de selecionar os Fornecedores.";
            }
        }
    }
}

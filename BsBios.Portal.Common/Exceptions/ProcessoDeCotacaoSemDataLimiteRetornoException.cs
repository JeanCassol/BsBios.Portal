using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoSemDataLimiteRetornoException: Exception
    {
        public override string Message
        {
            get
            {
                return "Não é possível inicar o Processo de Cotação enquanto a Data Limite de Retorno não estiver preenchida.";
            }
        }
    }
}

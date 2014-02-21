using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class FornecedorSemCondicaoGeradaNoSapException : Exception
    {
        public override string Message
        {
            get { return "N�o foi gerada cota��o no SAP para todos os fornecedores selecionados no Processo de Cota��o."; }
        }
    }
}
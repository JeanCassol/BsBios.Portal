using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class FornecedorSemCondicaoGeradaNoSapException : Exception
    {
        public override string Message
        {
            get { return "Não foi gerada cotação no SAP para todos os fornecedores selecionados no Processo de Cotação."; }
        }
    }
}
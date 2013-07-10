using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class RequisicaoDeCompraComProcessoDeCotacaoBloqueioException: Exception
    {
        public override string Message
        {
            get { return "Não é possível bloquear uma requisição de compra que já está vinculada a um processo de cotação."; }
        }
    }
}
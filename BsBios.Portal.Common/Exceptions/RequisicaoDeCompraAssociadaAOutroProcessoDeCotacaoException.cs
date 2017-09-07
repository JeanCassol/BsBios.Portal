using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class RequisicaoDeCompraAssociadaAOutroProcessoDeCotacaoException: Exception
    {
        private readonly string _numeroDaRequisicao;
        private readonly string _numeroDoItem;
        
        public RequisicaoDeCompraAssociadaAOutroProcessoDeCotacaoException(string numeroDaRequisicao, string numeroDoItem)
        {
            _numeroDaRequisicao = numeroDaRequisicao;
            _numeroDoItem = numeroDoItem;
        }

        public override string Message
        {
            get { return "O Item nº " + _numeroDaRequisicao + " da  Requisição de Compra de nº " + _numeroDoItem +
                " já está sendo utilizada por outro Processo de Cotação."; }
        }
    }
}
using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class RequisicaoDeCompraAssociadaAOutroProcessoDeCotacaoException: Exception
    {
        private readonly string _numeroDaRequisicao;
        private readonly string _numeroDoItem;
        private readonly int _idProcessoCotacao;
        
        public RequisicaoDeCompraAssociadaAOutroProcessoDeCotacaoException(string numeroDaRequisicao, string numeroDoItem, int idProcessoCotacao)
        {
            _numeroDaRequisicao = numeroDaRequisicao;
            _numeroDoItem = numeroDoItem;
            _idProcessoCotacao = idProcessoCotacao;
        }

        public override string Message
        {
            get { return "O Item nº " + _numeroDaRequisicao + " da  Requisição de Compra de nº " + _numeroDoItem +
                " já está sendo utilizada pelo Processo de Cotação " + Convert.ToString(_idProcessoCotacao); }
        }
    }
}
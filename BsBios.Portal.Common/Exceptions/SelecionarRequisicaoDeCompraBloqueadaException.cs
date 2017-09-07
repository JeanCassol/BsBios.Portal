using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class SelecionarRequisicaoDeCompraBloqueadaException : Exception
    {
        private readonly string _numeroDaRequisicao;
        private readonly string _numeroDoItem;
        public SelecionarRequisicaoDeCompraBloqueadaException(string numeroDaRequisicao, string numeroDoItem)
        {
            _numeroDaRequisicao = numeroDaRequisicao;
            _numeroDoItem = numeroDoItem;
        }

        public override string Message
        {
            get { return String.Format("O item {0} da Requisição de Compra {1} não pode ser selecionado, pois está bloqueado.", _numeroDoItem, _numeroDaRequisicao); }
        }
    }
}
using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ExcluirQuotaComAgendamentoException : Exception
    {
        private readonly string _nomeDoFornecedor;
        public ExcluirQuotaComAgendamentoException(string nomeDoFornecedor)
        {
            _nomeDoFornecedor = nomeDoFornecedor;
        }

        public override string Message
        {
            get { return "Não é permitido excluir a quota do fornecedor "  +  _nomeDoFornecedor + 
                         ", pois já possui agendamentos." ; }
        }
    }
}
using System;

namespace BsBios.Portal.Domain.Entities
{
    public class FornecedorParticipante
    {
        public virtual int Id { get; protected set; }
        public virtual ProcessoDeCotacao ProcessoDeCotacao { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual Cotacao Cotacao { get; protected set; }   
        
        protected FornecedorParticipante(){}

        public FornecedorParticipante(ProcessoDeCotacao processoDeCotacao, Fornecedor fornecedor)
        {
            ProcessoDeCotacao = processoDeCotacao;
            Fornecedor = fornecedor;
        }

        public virtual Cotacao InformarCotacao(Cotacao cotacao)
        {
            Cotacao = cotacao;
            return Cotacao;
        }
    }
}

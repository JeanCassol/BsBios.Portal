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

        public virtual Cotacao InformarCotacao(CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoDoIncoterm,
            decimal valorTotalComImpostos, decimal? mva, decimal quantidadeDisponivel,
            DateTime prazoDeEntrega, string observacoes)
        {
            if (Cotacao == null)
            {
                Cotacao = new Cotacao(condicaoDePagamento, incoterm, descricaoDoIncoterm, 
                    valorTotalComImpostos, mva,quantidadeDisponivel, prazoDeEntrega, observacoes);
                
            }
            else
            {
                Cotacao.Atualizar(valorTotalComImpostos, condicaoDePagamento, incoterm, 
                    descricaoDoIncoterm, mva,quantidadeDisponivel, prazoDeEntrega, observacoes);
            }

            return Cotacao;
        }
    }
}

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
            decimal valorTotalSemImpostos, decimal? valorTotalComImpostos, decimal? mva)
        {
            if (Cotacao == null)
            {
                Cotacao = new Cotacao(condicaoDePagamento, incoterm, descricaoDoIncoterm, valorTotalSemImpostos, valorTotalComImpostos, mva);
                
            }
            else
            {
                Cotacao.Atualizar(valorTotalSemImpostos, valorTotalComImpostos, condicaoDePagamento, incoterm, descricaoDoIncoterm, mva);
            }

            return Cotacao;
        }
    }
}

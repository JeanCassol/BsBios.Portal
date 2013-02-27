namespace BsBios.Portal.Domain.Entities
{
    public class Cotacao
    {
        //public virtual ProcessoDeCotacao ProcessoDeCotacao { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual bool Selecionada { get; protected set; }
        public virtual decimal? ValorUnitario { get; protected set; }
        public virtual decimal? QuantidadeAdquirida { get; protected set; }
        public virtual CondicaoDePagamento CondicaoDePagamento { get; protected set; }
        public virtual Iva Iva { get; protected set; }
        public virtual Incoterm Incoterm { get; protected set; }
        public virtual string DescricaoIncoterm{ get; protected set; }

        protected Cotacao(){}

        public Cotacao(/*ProcessoDeCotacao processoDeCotacao, */Fornecedor fornecedor)
        {
            //ProcessoDeCotacao = processoDeCotacao;
            Fornecedor = fornecedor;
            Selecionada = false;
        }

        public virtual void Atualizar(decimal valorUnitario, Incoterm incoterm, string descricaoIncoterm)
        {
            ValorUnitario = valorUnitario;
            Incoterm = incoterm;
            DescricaoIncoterm = descricaoIncoterm;
        }
        public virtual void Selecionar(decimal quantidadeAdquirida, Iva iva, CondicaoDePagamento condicaoDePagamento)
        {
            Selecionada = true;
            QuantidadeAdquirida = quantidadeAdquirida;
            Iva = iva;
            CondicaoDePagamento = condicaoDePagamento;
        }

    }

   
}

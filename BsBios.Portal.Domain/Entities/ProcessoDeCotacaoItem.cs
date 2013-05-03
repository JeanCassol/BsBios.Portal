namespace BsBios.Portal.Domain.Entities
{
    public class ProcessoDeCotacaoItem
    {
        public ProcessoDeCotacao ProcessoDeCotacao { get; protected set; }
        public Produto Material { get; protected set; }
        public decimal Quantidade { get; protected set; }
        public UnidadeDeMedida UnidadeDeMedida { get; protected set; }

        protected ProcessoDeCotacaoItem(){}

        protected ProcessoDeCotacaoItem(ProcessoDeCotacao processoDeCotacao, Produto material, decimal quantidade
            , UnidadeDeMedida unidadeDeMedida)
        {
            ProcessoDeCotacao = processoDeCotacao;
            Material = material;
            Quantidade = quantidade;
            UnidadeDeMedida = unidadeDeMedida;
        }
    }
}

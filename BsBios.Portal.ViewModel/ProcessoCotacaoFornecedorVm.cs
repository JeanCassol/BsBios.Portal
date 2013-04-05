namespace BsBios.Portal.ViewModel
{
    public class ProcessoCotacaoFornecedorVm:ListagemVm
    {
        public int IdFornecedorParticipante { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Selecionado { get; set; }
        public decimal? ValorLiquido { get; set; }
        public decimal? ValorComImpostos { get; set; }
        public string VisualizadoPeloFornecedor { get; set; }
    }
}

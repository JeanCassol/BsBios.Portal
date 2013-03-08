namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// utiliza para carregar a tela de seleção das cotações vencedoras
    /// </summary>
    public class CotacaoSelecionarVm
    {
        public int IdCotacao { get; set; }
        public bool Selecionada { get; set; }
        public string Fornecedor { get; set; }
        public string CodigoIva { get; set; }
        public decimal? QuantidadeAdquirida { get; set; }
        public decimal? ValorLiquido { get; set; }
        public decimal? ValorComImpostos { get; set; }
        public string CondicaoDePagamento { get; set; }
        public string Incoterm { get; set; }
        public decimal? ValorIcms { get; set; }
        public decimal? ValorIcmsSt { get; set; }
        public decimal? ValorIpi { get; set; }
        public decimal? ValorPis { get; set; }
        public decimal? ValorCofins { get; set; }
    }
}

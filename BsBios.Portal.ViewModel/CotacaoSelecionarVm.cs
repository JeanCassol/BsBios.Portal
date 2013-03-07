namespace BsBios.Portal.ViewModel
{
    public class CotacaoSelecionarVm: CotacaoDadosVm
    {
        public int IdCotacao { get; set; }
        public bool Selecionada { get; set; }
        public string Fornecedor { get; set; }
        public string CodigoIva { get; set; }
        public decimal? QuantidadeAdquirida { get; set; }
    }
}

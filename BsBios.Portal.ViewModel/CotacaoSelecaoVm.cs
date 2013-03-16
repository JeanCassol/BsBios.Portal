namespace BsBios.Portal.ViewModel
{
    public class CotacaoFreteSelecaoVm
    {
        public int IdCotacao { get; set; }
        public bool Selecionada { get; set; }
        public decimal? QuantidadeAdquirida { get; set; }
    }

    public class CotacaoMaterialSelecaoVm: CotacaoFreteSelecaoVm
    {
        public string CodigoIva { get; set; }
    }
}

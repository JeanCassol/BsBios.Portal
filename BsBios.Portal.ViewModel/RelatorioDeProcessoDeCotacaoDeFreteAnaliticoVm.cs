namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm : ProcessoDeCotacaoDeFreteBaseVm
    {
        public int IdDoProcessoDeCotacao { get; set; }
        public string Transportadora { get; set; }
        public bool Visualizado { get; set; }
        public bool Selecionado { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public decimal QuantidadeLiberada { get; set; }
        public decimal ValorComImpostos { get; set; }

    }
}

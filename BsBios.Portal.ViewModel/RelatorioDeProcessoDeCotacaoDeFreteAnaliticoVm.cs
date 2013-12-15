namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm
    {
        public string Status { get; set; }
        public string Material { get; set; }
        public string UnidadeDeMedida { get; set; }
        public string Itinerario { get; set; }
        public string Classificacao { get; set; }
        public string Transportadora { get; set; }
        public string Selecionado { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public decimal QuantidadeLiberada { get; set; }
        public decimal ValorComImpostos { get; set; }

    }
}

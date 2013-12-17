namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeOrdemDeTransporteSinteticoVm
    {
        public string Status { get; set; }
        public string Material { get; set; }
        public string FornecedorDaMercadoria { get; set; }
        public string Transportadora { get; set; }
        public string UnidadeDeMedida { get; set; }
        public string Classificacao { get; set; }
        public decimal Cadencia { get; set; }
        public decimal QuantidadeLiberada { get; set; }
        public decimal QuantidadeDeTolerancia { get; set; }
        public decimal QuantidadeEmTransito { get; set; }
        public decimal QuantidadeRealizada { get; set; }
        public decimal QuantidadePendente { get; set; }

    }

}

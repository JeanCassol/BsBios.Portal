namespace BsBios.Portal.ViewModel
{
    public class MonitorDeOrdemDeTransporteVm
    {
        public string Material { get; set; }
        public string Fornecedor { get; set; }
        public decimal QuantidadeLiberada { get; set; }
        public decimal QuantidadeEmTransito { get; set; }
        public decimal QuantidadeComPrevisaoDeChegadaNoDia { get; set; }
        public string QuantidadePendente { get; set; }

    }
}
namespace BsBios.Portal.ViewModel
{
    public class QuotaConsultarVm
    {
        public int IdQuota { get; set; }
        public string CodigoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public int CodigoMaterial { get; set; }
        public string DescricaoMaterial { get; set; }
        public int CodigoFluxoCarga { get; set; }
        public string DescricaoFluxoCarga { get; set; }
        public decimal Peso { get; set; }
        public decimal PesoAgendado { get; set; }
    }
}

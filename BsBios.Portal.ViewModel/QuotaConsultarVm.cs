namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// Utilizada no grid de quotas da tela de cadastro
    /// </summary>
    public class QuotaConsultarVm
    {
        public string CodigoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public int CodigoMaterial { get; set; }
        public string DescricaoMaterial { get; set; }
        public int FluxoCarga { get; set; }
        public string DescricaoFluxoCarga { get; set; }
        public decimal  Peso { get; set; }
    }
}

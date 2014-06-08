namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// classe utilizada para salvar as quotas
    /// </summary>
    public class QuotaSalvarVm
    {
        public int CodigoMaterial { get; set; }
        public int FluxoDeCarga { get; set; }
        public string CodigoFornecedor { get; set; }
        public decimal Peso { get; set; }
    }
}

using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class QuotaPlanejadoRealizadoVm
    {
        [DisplayName("Terminal")]
        public string CodigoTerminal { get; set; }
        [DisplayName("Fornecedor")]
        public string NomeDoFornecedor { get; set; }
        [DisplayName("Fluxo de Carga")]
        public string FluxoDeCarga { get; set; }
        public decimal Quota { get; set; }
        [DisplayName("Peso")]
        public decimal PesoRealizado { get; set; }
    }

    public class QuotaPlanejadoRealizadoPorDataVm: QuotaPlanejadoRealizadoVm
    {
        public string Data { get; set; }
    }
}

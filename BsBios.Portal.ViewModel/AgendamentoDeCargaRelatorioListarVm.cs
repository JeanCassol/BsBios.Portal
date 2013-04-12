using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class AgendamentoDeCargaRelatorioListarVm
    {
        public string Terminal { get; set; }
        public string Data { get; set; }
        public string Fornecedor { get; set; }
        [DisplayName("Fluxo de Carga")]
        public string FluxoDeCarga { get; set; }
        public string Placa { get; set; }
        public string Material { get; set; }
        public decimal Peso { get; set; }
    }
}

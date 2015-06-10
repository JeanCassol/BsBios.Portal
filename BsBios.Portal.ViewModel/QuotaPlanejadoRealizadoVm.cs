using System.Collections.Generic;
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
        public string Material { get; set; }
        public decimal Quota { get; set; }
        [DisplayName("Peso")]
        public decimal PesoRealizado { get; set; }
        public decimal PesoNaoRealizado { get; set; }
    }

    public class QuotaPlanejadoRealizadoPorDataVm: QuotaPlanejadoRealizadoVm
    {
        public string Data { get; set; }
    }

    public class QuotaPlanejadoRealizadoTotalVm
    {
        public decimal Quota { get; set; }
        public decimal PesoRealizado { get; set; }
        public decimal PesoNaoRealizado { get; set; }
    }

    public class RelatorioDeQuotaPlanejadoVersusRealizadoVm
    {
        public List<QuotaPlanejadoRealizadoVm> Quotas { get; set; }
        public QuotaPlanejadoRealizadoTotalVm Total { get; set; }

    }

    public class RelatorioDeQuotaPlanejadoVersusRealizadoPorDataVm
    {
        public List<QuotaPlanejadoRealizadoPorDataVm> Quotas { get; set; }
        public QuotaPlanejadoRealizadoTotalVm Total { get; set; }

    }


}

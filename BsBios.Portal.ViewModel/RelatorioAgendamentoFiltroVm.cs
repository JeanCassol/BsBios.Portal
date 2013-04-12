using System;
using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioAgendamentoFiltroVm
    {
        [DisplayName("Terminal")]
        public string CodigoTerminal { get; set; }
        [DisplayName("Fluxo de Carga")]
        public int? CodigoFluxoDeCarga { get; set; }
        [DisplayName("Date De")]
        public DateTime? DataDe { get; set; }
        [DisplayName("Até")]
        public DateTime? DataAte { get; set; }
        public string CodigoFornecedor { get; set; }
        public string Fornecedor { get; set; }
        public string Placa { get; set; }
    }
}

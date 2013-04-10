using System;
using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioAgendamentoFiltroVm
    {
        [DisplayName("Terminal")]
        public string CodigoTerminal { get; set; }
        [DisplayName("Fluxo de Carga")]
        public int CodigoFluxoDeCarga { get; set; }
        public DateTime? Data { get; set; }
        public string CodigoFornecedor { get; set; }
    }
}

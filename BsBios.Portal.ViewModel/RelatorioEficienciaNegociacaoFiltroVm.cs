using System;
using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioEficienciaNegociacaoFiltroVm
    {
        [DisplayName("Date De")]
        public DateTime? DataDe { get; set; }
        [DisplayName("Até")]
        public DateTime? DataAte { get; set; }
        public string LoginComprador { get; set; }
        public string Comprador { get; set; }
    }
}

using System;
using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioEficienciaNegociacaoFiltroVm
    {
        [DisplayName("Data de Fechamento De")]
        public DateTime? DataDe { get; set; }
        [DisplayName("Data de Fechamento Até")]
        public DateTime? DataAte { get; set; }
        public string LoginComprador { get; set; }
        public string Comprador { get; set; }
    }
}

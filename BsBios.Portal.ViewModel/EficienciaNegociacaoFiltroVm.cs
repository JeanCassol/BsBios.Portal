using System;
using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class EficienciaNegociacaoFiltroVm
    {
        [DisplayName("Data de Fechamento De")]
        public DateTime? DataDeFechamentoInicial { get; set; }
        [DisplayName("Data de Fechamento Até")]
        public DateTime? DataDeFechamentoFinal { get; set; }
        public string LoginComprador { get; set; }
        //public string Comprador { get; set; }
    }
}

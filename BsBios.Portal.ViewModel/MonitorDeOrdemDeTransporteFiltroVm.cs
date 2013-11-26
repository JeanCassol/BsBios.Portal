using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class MonitorDeOrdemDeTransporteFiltroVm
    {
        
        [Required(ErrorMessage = "Data Inicial é obrigatório")]
        [Display(Name = "Data Inicial")]
        public string DataInicial { get; set; }

        [Required(ErrorMessage = "Data Final é obrigatório")]
        [Display(Name = "Data Final")]
        public string DataFinal { get; set; }
    }

    public class MonitorOrdemDeTransporteParametroVm : MonitorDeOrdemDeTransporteFiltroVm
    {
        [Required(ErrorMessage = "Intervalo de Atualização é obrigatório")]
        [Display(Name = "Intervalo de Atualização (minutos)")]
        public int InterValoDeAtualizacao { get; set; }
    }
}
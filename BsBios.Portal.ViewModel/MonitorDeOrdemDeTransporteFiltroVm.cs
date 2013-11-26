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
        [Display(Name = "Intervalo de Atualização (segundos)")]
        [Range(1,1000000,ErrorMessage = "Intervalo deve ser um valor entre 1 e 1.000.000")]
        public int InterValoDeAtualizacao { get; set; }
    }
}
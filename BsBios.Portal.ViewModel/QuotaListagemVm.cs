using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// classe utilizada como modelo do MVC para a tela de cadastro de quotas e no relatório de Listagem de Quotas
    /// </summary>
    public class QuotaListagemVm
    {
        [Display(Name = "Terminal")]
        public string Terminal { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public string Data { get; set; }

        [Display(Name = "Fornecedor")]
        public string Fornecedor { get; set; }

        [Display(Name = "Material")]
        public string Material { get; set; }

        [Display(Name = "Fluxo de Carga")]
        public string FluxoDeCarga { get; set; }

        [Display(Name = "Peso")]
        public decimal Peso { get; set; }

        [Display(Name = "Peso Agendado")]
        public decimal PesoAgendado { get; set; }

    }
}

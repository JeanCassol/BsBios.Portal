using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// classe utilizada como modelo do MVC para a tela de cadastro
    /// </summary>
    public class QuotaCadastroVm
    {
        [Required(ErrorMessage = "Terminal é obrigatório")]
        [Display(Name = "Terminal")]
        public string Terminal { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data é obrigatório")]
        [Display(Name = "Data")]
        public string Data { get; set; }

        [Required(ErrorMessage = "Fornecedor é obrigatório")]
        [Display(Name = "Fornecedor")]
        public string Fornecedor { get; set; }

        [Required(ErrorMessage = "Material é obrigatório")]
        [Display(Name = "Material")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Fluxo de Carga é obrigatório")]
        [Display(Name = "Fluxo de Carga")]
        public string FluxoDeCarga { get; set; }

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Display(Name = "Peso")]
        public decimal Peso { get; set; }
    }
}

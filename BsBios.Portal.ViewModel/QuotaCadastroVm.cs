using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class QuotaCadastroVm
    {
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

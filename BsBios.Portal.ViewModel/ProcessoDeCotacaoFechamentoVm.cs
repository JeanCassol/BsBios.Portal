using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoFechamentoVm
    {
        public int IdProcessoCotacao { get; set; }
        [Required(ErrorMessage = "Justificativa é obrigatório")]
        [Display(Name = "Justificativa")]
        public string Justificativa { get; set; }
    }
}

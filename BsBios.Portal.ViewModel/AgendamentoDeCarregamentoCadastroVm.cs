using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class AgendamentoDeCarregamentoCadastroVm : AgendamentoDeCargaCadastroVm
    {

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Display(Name = "Peso")]
        public decimal Peso { get; set; }
    }
}

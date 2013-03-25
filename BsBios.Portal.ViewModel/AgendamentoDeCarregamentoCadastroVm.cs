using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class AgendamentoDeCarregamentoCadastroVm
    {
        public int IdAgendamento { get; set; }
        public int IdQuota { get; set; }

        [Required(ErrorMessage = "Placa é obrigatório")]
        [Display(Name = "Placa")]
        [MaxLength(8)]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Display(Name = "Peso")]
        public decimal Peso { get; set; }
    }
}

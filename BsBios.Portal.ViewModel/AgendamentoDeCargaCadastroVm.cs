using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public abstract class AgendamentoDeCargaCadastroVm
    {
        public int IdAgendamento { get; set; }
        public int IdQuota { get; set; }

        [Required(ErrorMessage = "Placa é obrigatório")]
        [Display(Name = "Placa")]
        [MaxLength(8)]
        public string Placa { get; set; }

        public string ViewDeCadastro { get; set; }

        public bool PermiteEditar { get; set; }
        public bool PermiteRealizar { get; set; }
    }
}

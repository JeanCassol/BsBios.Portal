using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class AgendamentoDeCarregamentoCadastroVm
    {
        public int Id { get; set; }
        public string CodigoTerminal { get; set; }
        public string Data { get; set; }
        public string CodigoMaterial { get; set; }

        [Required(ErrorMessage = "Placa é obrigatório")]
        [Display(Name = "Placa")]
        [MaxLength(8)]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Display(Name = "Peso")]
        public decimal Peso { get; set; }
    }
}

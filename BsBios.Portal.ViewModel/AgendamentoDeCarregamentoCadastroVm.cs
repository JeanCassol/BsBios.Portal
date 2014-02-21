using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class AgendamentoDeCarregamentoCadastroVm : AgendamentoDeCargaCadastroVm
    {

        [Required(ErrorMessage = "Peso é obrigatório")]
        public decimal Peso { get; set; }
        [Required(ErrorMessage = "Motorista é obrigatório")]
        public string Motorista { get; set; }
        [Required(ErrorMessage = "Destino é obrigatório")]
        public string Destino { get; set; }

        public AgendamentoDeCarregamentoCabecalhoVm Cabecalho { get; set; }

    }

    public class AgendamentoDeCarregamentoCabecalhoVm
    {
        [Display(Name = "Transportadora: ")]
        public string Transportadora { get; set; }
        [Display(Name = "Material: ")]
        public string Material { get; set; }
        [Display(Name = "Fluxo de Carga: ")]
        public string FluxoDeCarga { get; set; }



        
    }
}

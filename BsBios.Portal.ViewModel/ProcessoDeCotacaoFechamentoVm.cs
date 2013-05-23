using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoFechamentoVm
    {
        public int IdProcessoCotacao { get; set; }
        [Required(ErrorMessage = "Texto de Cabeçalho é obrigatório")]
        [Display(Name = "Texto de Cabeçalho")]
        public string TextoDeCabecalho { get; set; }

        [Required(ErrorMessage = "Nota de Cabeçalho é obrigatório")]
        [Display(Name = "Nota de Cabeçalho")]
        public string NotaDeCabecalho { get; set; }

    }

}

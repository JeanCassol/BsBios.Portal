using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class AlterarSenhaVm
    {
        [Required]
        [Display(Name = "Usuário")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string SenhaAtual { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Nova")]
        public string SenhaNova { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da Senha")]
        [Compare("SenhaNova", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmacaoSenha{ get; set; } 
    }
}

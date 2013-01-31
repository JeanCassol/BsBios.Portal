using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class LoginVm
    {
        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; set; }
    }
}

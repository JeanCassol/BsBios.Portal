using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class LoginVm
    {
        [DataMember]
        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string Usuario { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; set; }
    }
}

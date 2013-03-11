using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class UsuarioCadastroVm : ListagemVm
    {
        [DataMember]
        [Display(Name = "Login: ")]
        public string Login { get; set; }
        [DataMember]
        [Display(Name = "Nome: ")]
        public string Nome { get; set; }
        [DataMember]
        [Display(Name = "E-mail: ")]
        public string Email { get; set; }
    }

    [CollectionDataContract]
    public class ListaUsuario:List<UsuarioCadastroVm>{}

    public class UsuarioConsultaVm: UsuarioCadastroVm
    {
        [Display(Name = "Status: ")]
        public string Status { get; set; }
    }
}

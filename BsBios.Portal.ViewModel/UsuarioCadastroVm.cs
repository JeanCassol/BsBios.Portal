using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class UsuarioCadastroVm
    {
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Email { get; set; }
    }

    [CollectionDataContract]
    public class ListaUsuario:List<UsuarioCadastroVm>{}
}

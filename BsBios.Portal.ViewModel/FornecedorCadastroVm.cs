using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class FornecedorCadastroVm:ListagemVm
    {
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Email { get; set; }
    }

    [CollectionDataContract]
    public class ListaFornecedores:List<FornecedorCadastroVm>{}
}

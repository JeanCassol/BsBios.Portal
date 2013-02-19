using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract(Name = "item")]
    public class FornecedorCadastroVm
    {
        [DataMember(Name = "lifnr")]
        public string CodigoSap { get; set; }
        [DataMember(Name = "name1")]
        public string Nome { get; set; }

        [DataMember]
        public string Email { get; set; }
    }

    [CollectionDataContract(Name = "mt_cadFornecedor_portal")]
    public class ListaFornecedores:List<FornecedorCadastroVm>{}
}

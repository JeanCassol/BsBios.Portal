using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class ProdutoFornecedorCadastroVm
    {
        [DataMember]
        public string CodigoProduto { get; set; }
        [DataMember]
        public string CodigoFornecedor { get; set; }
    }

    [CollectionDataContract]
    public class ListaProdutoFornecedor: List<ProdutoFornecedorCadastroVm>{}
}

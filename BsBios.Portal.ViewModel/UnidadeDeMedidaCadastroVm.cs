using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class UnidadeDeMedidaCadastroVm
    {
        [DataMember]
        public string CodigoInterno { get; set; }
        [DataMember]
        public string CodigoExterno { get; set; }
        [DataMember]
        public string Descricao { get; set; }
    }

    [CollectionDataContract]
    public class ListaUnidadesDeMedida:List<UnidadeDeMedidaCadastroVm>{}
}

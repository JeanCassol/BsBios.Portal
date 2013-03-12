using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class ItinerarioCadastroVm
    {
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Descricao { get; set; }
    }
    [CollectionDataContract]
    public class ListaItinerarios : List<ItinerarioCadastroVm> { }

}

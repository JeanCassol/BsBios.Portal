using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class TerminalCadastroVm
    {
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Cidade { get; set; }
    }

    [CollectionDataContract]
    public class ListaTerminal : List<TerminalCadastroVm> { }

}
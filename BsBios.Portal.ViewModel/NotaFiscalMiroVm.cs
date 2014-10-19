using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class NotaFiscalMiroVm
    {
        [DataMember]
        public string CnpjDoFornecedor { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string Serie { get; set; }
    }

    [CollectionDataContract]
    public class ListaDeNotaFiscalMiro : List<NotaFiscalMiroVm> { }
}

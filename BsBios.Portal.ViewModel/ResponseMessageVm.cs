using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BsBios.Portal.ViewModel
{
    //[Serializable, DataContract]
    public class Retorno
    {
        //[DataMember]
        public string Codigo { get; set; }
        //[DataMember]
        public string Texto { get; set; }
    }

    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/BsBios.Portal.ViewModel")]
    [DataContract]
    public class ApiResponseMessage
    {
        [DataMember]
        public Retorno Retorno { get; set; }
    }

    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/BsBios.Portal.ViewModel")]
    [DataContract]
    public class ProcessoDeCotacaoDeFreteFechamentoRetorno: ApiResponseMessage
    {
        //[DataMember]
        //public Retorno Retorno { get; set; }
        [DataMember]
        public ListaDeCondicaoDoSap Condicoes { get; set; }
    }

    [DataContract]
    public class CondicaoDoSap
    {
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string CodigoDoFornecedor { get; set; }
    }

    [CollectionDataContract]
    public class ListaDeCondicaoDoSap : List<CondicaoDoSap> { }

}

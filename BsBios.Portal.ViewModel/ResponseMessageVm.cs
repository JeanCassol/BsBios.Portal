using System;
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
    public class ApiResponseMessage
    {
        [DataMember]
        public Retorno Retorno { get; set; }
    }
}

using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{

    public class Retorno
    {
        [DataMember(Name = "retCodigo")]
        public string Codigo { get; set; }
        [DataMember(Name = "retTexto")]
        public string Texto { get; set; }
    }

    //[DataContract(Namespace = "http://portal.bsbios.com.br/")]
    [DataContract(Name = "mt_cadMaterial_portal_ret")]
    public class ApiResponseMessage
    {
        [DataMember(Name = "retorno")]
        public Retorno Retorno { get; set; }
    }
}

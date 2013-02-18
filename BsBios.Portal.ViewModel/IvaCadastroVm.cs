using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public  class IvaCadastroVm
    {
        [DataMember]
        public string CodigoSap { get; set; }
        [DataMember]
        public string Descricao { get; set; }
    }
}

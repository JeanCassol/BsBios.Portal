using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public  class IvaCadastroVm
    {
        [IgnoreDataMember]
        public int Id { get; set; }
        [DataMember]
        public string CodigoSap { get; set; }
        [DataMember]
        public string Descricao { get; set; }
    }
}

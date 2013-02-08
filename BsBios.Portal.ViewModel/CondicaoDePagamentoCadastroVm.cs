using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class CondicaoDePagamentoCadastroVm
    {
        [IgnoreDataMember]
        public int Id { get; set; }
        [DataMember]
        public string CodigoSap { get; set; }
        [DataMember]
        public string Descricao { get; set; }
    }
}

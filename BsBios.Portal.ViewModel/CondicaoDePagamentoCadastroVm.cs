using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class CondicaoDePagamentoCadastroVm
    {
        [DataMember]
        public string CodigoSap { get; set; }
        [DataMember]
        public string Descricao { get; set; }
    }
}

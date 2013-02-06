using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class FornecedorCadastroVm
    {
        [DataMember]
        public string CodigoSap { get; set; }
        [DataMember]
        public string Nome { get; set; }
    }
}

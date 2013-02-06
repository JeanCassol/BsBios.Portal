using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class ProdutoCadastroVm
    {
        [IgnoreDataMember]
        public int? Id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Código Sap é obrigatório")]
        [Display(Name = "Código Sap")]
        [DataType(DataType.Text)]
        public string CodigoSap { get; set;}

        [DataMember]
        [Required(ErrorMessage = "Descrição do Produto é obrigatória")]
        [Display(Name = "Descrição")]
        [DataType(DataType.Text)]
        public string Descricao { get; set; }
    }
}

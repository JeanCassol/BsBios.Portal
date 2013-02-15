using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract(Name = "item")]
    //[DataContract]
    public class ProdutoCadastroVm
    {
        [IgnoreDataMember]
        public int? Id { get; set; }

        //[DataMember]
        [DataMember(Name = "matnr")]
        [Required(ErrorMessage = "Código Sap é obrigatório")]
        [Display(Name = "Código Sap")]
        [DataType(DataType.Text)]
        public string CodigoSap { get; set;}

        //[DataMember]
        [DataMember(Name = "maktx")]
        [Required(ErrorMessage = "Descrição do Produto é obrigatória")]
        [Display(Name = "Descrição")]
        [DataType(DataType.Text)]
        public string Descricao { get; set; }

        [DataMember(Name = "mtart")]
        //[DataMember]
        public string Tipo { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract(Name = "item")]
    public class ProdutoCadastroVm:ListagemVm
    {
        [DataMember(Name = "matnr")]
        [Required(ErrorMessage = "Código Sap é obrigatório")]
        [Display(Name = "Código: ")]
        [DataType(DataType.Text)]
        public string Codigo { get; set; }

        [DataMember(Name = "maktx")]
        [Required(ErrorMessage = "Descrição do Produto é obrigatória")]
        [Display(Name = "Descrição: ")]
        [DataType(DataType.Text)]
        public string Descricao { get; set; }

        [DataMember(Name = "mtart")]
        [Display(Name = "Tipo: ")]
        public string Tipo { get; set; }
    }

    [CollectionDataContract(Name = "mt_cadMaterial_portal")]
    public class ListaProdutos : List<ProdutoCadastroVm>
    {
    }
}


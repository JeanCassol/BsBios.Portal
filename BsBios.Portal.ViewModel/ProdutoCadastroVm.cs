using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ProdutoCadastroVm
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Código Sap é obrigatório")]
        [Description("Código Sap")]
        [DataType(DataType.Text)]
        public string CodigoSap { get; set;}
        [Required(ErrorMessage = "Descrição do Produto é obrigatória")]
        [Description("Descrição")]
        [DataType(DataType.Text)]
        public string Descricao { get; set; }
    }
}

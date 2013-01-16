using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ProdutoCadastroVm
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "É necessário informar o Código Sap.")]
        [Description("Código Sap")]
        [DataType(DataType.Text)]
        public string CodigoSap { get; set;}
        [Required(ErrorMessage = "É necessário informar a Descrição do Produto.")]
        [Description("Descrição")]
        [DataType(DataType.Text)]
        public string Descricao { get; set; }
    }
}

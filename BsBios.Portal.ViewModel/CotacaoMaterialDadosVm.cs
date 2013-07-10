using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoMaterialDadosVm
    {
        [Required(ErrorMessage = "Condição de Pagamento é obrigatório")]
        [Display(Name = "Condição de Pagamento")]
        public string CodigoCondicaoPagamento { get; set; }
        
        [Required(ErrorMessage = "Incoterm é obrigatório")]
        [Display(Name = "Incoterm")]
        public string CodigoIncoterm { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Incoterm 2 é obrigatório")]
        [Display(Name = "Incoterm 2")]
        public string DescricaoIncoterm { get; set; }

    }

    public class CotacaoMaterialItemDadosVm:CotacaoDadosVm
    {
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor Líquido é obrigatório")]
        [Display(Name = "Preço")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "Prazo de Entrega é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayName("Prazo de Entrega")]
        public string PrazoDeEntrega { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "MVA")]
        public decimal? Mva { get; set; }
    }
}

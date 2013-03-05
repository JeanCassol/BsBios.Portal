using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// Utilizado para salvar as cotações do Fornecedor
    /// </summary>
    public class CotacaoInformarVm
    {
        public int IdProcessoCotacao { get; set; }
        public string CodigoFornecedor { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor Total (Sem Impostos) é obrigatório")]
        [Display(Name = "Valor Total (Sem Impostos)")]
        public decimal? ValorTotalSemImpostos { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Valor Total (Com Impostos)")]
        public decimal? ValorTotalComImpostos { get; set; }
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
        [DataType(DataType.Currency)]
        [Display(Name = "MVA")]
        public decimal? Mva { get; set; }
    }
}

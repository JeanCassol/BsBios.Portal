using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// Utilizado para salvar as cotações do Fornecedor
    /// </summary>
    public class CotacaoAtualizarVm
    {
        public int IdProcessoCotacao { get; set; }
        public int IdCotacao { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor Unitário é obrigatório")]
        [Display(Name = "Valor Unitário")]
        public decimal ValorUnitario { get; set; }
        [Required(ErrorMessage = "Iva é obrigatório")]
        [Display(Name = "Iva")]
        public string CodigoIva { get; set; }
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
}

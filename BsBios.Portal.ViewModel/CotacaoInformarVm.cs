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

        [Display(Name = "Possui Impostos: ")]
        public bool PossuiImpostos { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Valor do ICMS")]
        public decimal? IcmsValor { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do ICMS")]
        public decimal? IcmsAliquota { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Valor do ICMS ST")]
        public decimal? IcmsStValor { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do ICMS ST")]
        public decimal? IcmsStAliquota { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Valor do IPI")]
        public decimal? IpiValor { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do IPI")]
        public decimal? IpiAliquota { get; set; }

        [Display(Name = "Valor do Pis")]
        [DataType(DataType.Currency)]
        public decimal? PisValor { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do Pis")]
        public decimal? PisAliquota { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Valor do Cofins")]
        public decimal? CofinsValor { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do Cofins")]
        public decimal? CofinsAliquota { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoDadosVm
    {
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor Líquido é obrigatório")]
        [Display(Name = "Valor Líquido")]
        public decimal? ValorLiquido { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor com Impostos é obrigatório")]
        [Display(Name = "Valor Com Impostos")]
        public decimal? ValorComImpostos { get; set; }

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

        [DisplayName("Observações")]
        public string ObservacoesDoFornecedor { get; set; }

        [Required(ErrorMessage = "Quantidade Disponível é obrigatório")]
        [DisplayName("Quantidade Disponível")]
        public decimal? QuantidadeDisponivel { get; set; }

        [Required(ErrorMessage = "Prazo de Entrega é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayName("Prazo de Entrega")]
        public string PrazoDeEntrega { get; set; }


        [DataType(DataType.Currency)]
        [Display(Name = "MVA")]
        public decimal? Mva { get; set; }

        //[Display(Name = "Possui Impostos: ")]
        //public bool PossuiImpostos { get; set; }

        [Required(ErrorMessage = "Valor do ICMS é obrigatório")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor do ICMS")]
        public decimal? IcmsValor { get; set; }

        [Required(ErrorMessage = "Alíquota do ICMS é obrigatório")]
        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do ICMS")]
        public decimal? IcmsAliquota { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor do ICMS ST é obrigatório")]
        [Display(Name = "Valor do ICMS ST")]
        public decimal? IcmsStValor { get; set; }

        [Required(ErrorMessage = "Alíquota do ICMS ST é obrigatório")]
        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do ICMS ST")]
        public decimal? IcmsStAliquota { get; set; }

        [Required(ErrorMessage = "Valor do IPI é obrigatório")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor do IPI")]
        public decimal? IpiValor { get; set; }

        [Required(ErrorMessage = "Alíquota do IPI é obrigatório")]
        [DataType(DataType.Currency)]
        [Display(Name = "Alíquota do IPI")]
        public decimal? IpiAliquota { get; set; }

        [Required(ErrorMessage = "Alíquota do Pis/Cofins é obrigatório")]
        [Display(Name = "Alíquota Pis / Cofins")]
        [DataType(DataType.Currency)]
        public decimal? PisCofinsAliquota { get; set; }

        //[Display(Name = "Valor do Pis")]
        //[DataType(DataType.Currency)]
        //public decimal? PisValor { get; set; }

        //[DataType(DataType.Currency)]
        //[Display(Name = "Alíquota do Pis")]
        //public decimal? PisAliquota { get; set; }

        //[DataType(DataType.Currency)]
        //[Display(Name = "Valor do Cofins")]
        //public decimal? CofinsValor { get; set; }

        //[DataType(DataType.Currency)]
        //[Display(Name = "Alíquota do Cofins")]
        //public decimal? CofinsAliquota { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoImpostosVm
    {
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

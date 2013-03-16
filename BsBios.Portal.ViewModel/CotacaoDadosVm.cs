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

        [DisplayName("Observações")]
        public string ObservacoesDoFornecedor { get; set; }

        [Required(ErrorMessage = "Quantidade Disponível é obrigatório")]
        [DisplayName("Quantidade Disponível")]
        public decimal? QuantidadeDisponivel { get; set; }

        public CotacaoImpostosVm  Impostos { get; set; }
    }
}

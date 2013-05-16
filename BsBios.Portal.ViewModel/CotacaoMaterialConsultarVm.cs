using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoMaterialConsultarCadastroVm
    {
        public int IdProcessoCotacao { get; set; }
        public int IdCotacao { get; set; }
        public string CodigoFornecedor { get; set; }

        [Display(Name = "Condição de Pagamento: ")]
        public string CondicaoPagamento { get; set; }

        [Display(Name = "Incoterm: ")]
        public string Incoterm { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Incoterm 2: ")]
        public string DescricaoIncoterm { get; set; }

        [Display(Name = "Tipo de Frete: ")]
        public string TipoDeFrete { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoCotacaoMaterialCadastroVm
    {
        public int? Id { get; set; }
        [Display(Name = "Status: ")]
        public string DescricaoStatus { get; set; }

        //[Required(ErrorMessage = "Material é obrigatório")]
        //[Display(Name = "Material")]
        public string CodigoMaterial { get; set; }
        //public string DescricaoMaterial { get; set; }

        //[Display(Name = "Quantidade")]
        //[Required(ErrorMessage = "Quantidade do Material é obrigatória")]
        //public int QuantidadeMaterial { get; set; }

        //[Required(ErrorMessage = "Data de Início do Leilão é obrigatório")]
        //[Display(Name = "Início do Leilão")]
        //[DataType(DataType.Date)]
        //public string DataInicioLeilao { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Término do Leilão")]
        [Required(ErrorMessage = "Data de Término do Leilão é obrigatório")]
        public string DataTerminoLeilao { get; set; }

        //[DataType(DataType.Date)]
        //[Required(ErrorMessage = "Data Inicial de Validade da Cotação é obrigatório")]
        //[Display(Name = "Data Inicial de Validade" )]
        //public string DataValidadeCotacaoInicial { get; set; }

        //[DataType(DataType.Date)]
        //[Required(ErrorMessage = "Data Final de Validade da Cotação é obrigatório")]
        //[Display(Name = "Data Final de Validade")]
        //public string DataValidadeCotacaoFinal { get; set; }

        //[DataType(DataType.MultilineText)]
        //[Display(Name = "Requisitos")]
        //[Required(ErrorMessage = "Requisitos é obrigatório")]
        //public string Requisitos { get; set; }

        //[DataType(DataType.MultilineText)]
        //[Display(Name = "Observações")]
        //public string Observacoes { get; set; }

        public RequisicaoDeCompraVm RequisicaoDeCompraVm { get; set; }
    }
}

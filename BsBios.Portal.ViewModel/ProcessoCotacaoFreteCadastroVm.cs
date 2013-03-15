using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoCotacaoFreteCadastroVm
    {
        public int? Id { get; set; }
        [Display(Name = "Status")]
        public string DescricaoStatus { get; set; }

        public string CodigoMaterial { get; set; }
        [Required(ErrorMessage = "Material é obrigatório")]
        [Display(Name = "Material")]
        public string DescricaoMaterial { get; set; }

        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "Quantidade do Material é obrigatória")]
        public int QuantidadeMaterial { get; set; }

        [Display(Name = "Unidade de Medida")]
        [Required(ErrorMessage = "Unidade de Medida é obrigatória")]
        public string CodigoUnidadeMedida { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Limite de Retorno")]
        [Required(ErrorMessage = "Data Limite de Retorno é obrigatório")]
        public string DataLimiteRetorno { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data Inicial de Validade da Cotação é obrigatório")]
        [Display(Name = "Data Inicial de Validade")]
        public string DataValidadeCotacaoInicial { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data Final de Validade da Cotação é obrigatório")]
        [Display(Name = "Data Final de Validade")]
        public string DataValidadeCotacaoFinal { get; set; }

        [Required(ErrorMessage = "Itinerário é obrigatório")]
        public int CodigoItinerario { get; set; }
        [Display(Name = "Itinerário")]
        public string DescricaoItinerario { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Requisitos")]
        [Required(ErrorMessage = "Requisitos é obrigatório")]
        public string Requisitos { get; set; }

    }
}

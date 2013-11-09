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
        public decimal QuantidadeMaterial { get; set; }

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

        public string CodigoItinerario { get; set; }
        [Display(Name = "Itinerário")]
        [Required(ErrorMessage = "Itinerário é obrigatório")]
        public string DescricaoItinerario { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Requisitos")]
        [Required(ErrorMessage = "Requisitos é obrigatório")]
        public string Requisitos { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Número do Contrato")]
        public string NumeroDoContrato { get; set; }

        [Display(Name = "Classificação")]
        public bool Classificacao { get; set; }

        [Display(Name = "Cadência")]
        [Required(ErrorMessage = "Cadência é obrigatório")]
        //[Required]
        public decimal Cadencia { get; set; }

        public string CodigoDoFornecedorDaMercadoria { get; set; }

        [Display(Name = "Fornecedor da Mercadoria")]
        [Required(ErrorMessage = "Fornecedor da Mercadoria é obrigatório")]
        public string FornecedorDaMercadoria { get; set; }


        public string CodigoDoMunicipioDeOrigem { get; set; }

        [Display(Name = "Município de Origem")]
        [Required(ErrorMessage = "Município de Origem é obrigatório")]
        public string NomeDoMunicipioDeOrigem { get; set; }

        public string CodigoDoMunicipioDeDestino { get; set; }

        [Display(Name = "Município de Destino")]
        [Required(ErrorMessage = "Município de Destino é obrigatório")]
        public string NomeDoMunicipioDeDestino { get; set; }

        public string CodigoDoDeposito { get; set; }

        [Display(Name = "Depósito")]
        public string Deposito { get; set; }


    }
}

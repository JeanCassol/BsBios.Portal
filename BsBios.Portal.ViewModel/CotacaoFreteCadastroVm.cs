using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoFreteCadastroVm
    {
        public int? Id { get; set; }
        [Description("Status")]
        public string DescricaoStatus { get; set; }

        [Required(ErrorMessage="Material é obrigatório")]
        public int IdMaterial { get; set; }
        [Description("Material")]
        public string DescricaoMaterial { get; set; }

        [Description("Quantidade")]
        [Required(ErrorMessage = "Quantidade do Material é obrigatória")]
        public int QuantidadeMaterial { get; set; }

        [DataType(DataType.Date)]
        [Description("Data de Início do Leilão")]
        [Required(ErrorMessage = "Data de Início do Leilão é obrigatório")]
        public DateTime DataInicioLeilao { get; set; }

        [DataType(DataType.Date)]
        [Description("Data de Término do Leilão")]
        [Required(ErrorMessage = "Data de Término do Leilão é obrigatório")]
        public DateTime DataTerminoLeilao { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data de Validade Inicial da Cotação é obrigatório")]
        public DateTime DataValidadeCotacaoInicial { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data de Validade Final da Cotação é obrigatório")]
        public DateTime DataValidadeCotacaoFinal { get; set; }

        [Required(ErrorMessage = "Centro é obrigatório")]
        public int IdCentro { get; set; }
        [Description("Centro")]
        public string DescricaoCentro { get; set; }

        [Required(ErrorMessage = "Itinerário é obrigatório")]
        public int IdItinerario { get; set; }
        [Description("Itinerário")]
        public string DescricaoItinerario { get; set; }

        [DataType(DataType.MultilineText)]
        [Description("Requisitos")]
        [Required(ErrorMessage = "Requisitos é obrigatório")]
        public string Requisitos { get; set; }

        [DataType(DataType.MultilineText)]
        [Description("Observações")]
        public string Observacoes { get; set; }
    }
}

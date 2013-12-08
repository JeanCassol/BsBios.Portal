using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
    {
        [DisplayName("Status")]
        public int? StatusDoProcessoDeCotacao { get; set; }
        [DisplayName("Data de Validade Inicial")]
        public string DataDeValidadeInicial { get; set; }
        [DisplayName("Data de Validade Final")]
        public string DataDeValidadeFinal { get; set; }
        [DisplayName("Classificação")]
        public bool Classificacao { get; set; }
        public string CodigoDoMaterial { get; set; }
        [DisplayName("Material")]
        public string DescricaoDoMaterial { get; set; }
        public string CodigoDaTransportadora { get; set; }
        [DisplayName("Transportadora")]
        public string NomeDaTransportadora { get; set; }
        public string CodigoDoItinerario { get; set; }
        [DisplayName("Itinerário")]
        public string DescricaoDoItinerario { get; set; }
        public string CodigoDoFornecedorDaMercadoria{ get; set; }
        [DisplayName("Fornecedor da Mercadoria")]
        public string NomeDoFornecedorDaMercadoria { get; set; }


    }
}

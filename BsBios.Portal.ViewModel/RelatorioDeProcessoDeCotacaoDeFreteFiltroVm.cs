using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeProcessoDeCotacaoDeFreteFiltroVm : RelatorioDeOrdemDeTransporteFiltroVm
    {
        [DisplayName("Fornecedores Selecionados")]
        public int SelecaoDeFornecedores { get; set; }
        [DisplayName("Classificação")]
        public int Classificacao { get; set; }
        public string CodigoDoItinerario { get; set; }
        [DisplayName("Itinerário")]
        public string DescricaoDoItinerario { get; set; }

        public string DataDeFechamento { get; set; }
        [DisplayName("Terminal")]
        public string CodigoDoTerminal { get; set; }

    }
}

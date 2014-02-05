using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeOrdemDeTransporteFiltroVm
    {
        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Data de Validade Inicial")]
        public string DataDeValidadeInicial { get; set; }
        [DisplayName("Data de Validade Final")]
        public string DataDeValidadeFinal { get; set; }
        [DisplayName("Código do Material")]
        public string CodigoDoMaterial { get; set; }
        [DisplayName("Nome do Material")]
        public string DescricaoDoMaterial { get; set; }
        [DisplayName("Código da Transportadora")]
        public string CodigoDaTransportadora { get; set; }
        [DisplayName("Nome da Transportadora")]
        public string NomeDaTransportadora { get; set; }
        [DisplayName("Código do Fornecedor da Mercadoria")]
        public string CodigoDoFornecedorDaMercadoria{ get; set; }
        [DisplayName("Nome do Fornecedor da Mercadoria")]
        public string NomeDoFornecedorDaMercadoria { get; set; }
        [DisplayName("Terminal")]
        public string CodigoTerminal { get; set; }
        public string Terminal { get; set; }
    }
}

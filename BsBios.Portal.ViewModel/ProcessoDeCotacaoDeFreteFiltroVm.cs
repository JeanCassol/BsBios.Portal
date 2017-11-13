using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoDeFreteFiltroVm : ProcessoCotacaoFiltroVm
    {
        [DisplayName("Número do Contrato")]
        public string NumeroDoContrato { get; set; }
        [DisplayName("Nome do Fornecedor da Mercadoria")]
        public string NomeDoFornecedorDaMercadoria { get; set; }
        [DisplayName("Município de Origem")]
        public string CodigoDoMunicipioDeOrigem { get; set; }
        [DisplayName("Terminal")]
        public string CodigoDoTerminal { get; set; }
    }
}
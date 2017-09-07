using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoCotacaoFiltroVm
    {
        public string CodigoFornecedor { get; set; }
        //1 - material; 2 - frete
        public int TipoDeCotacao { get; set; }
        [DisplayName("Código")]
        public string CodigoProduto { get; set; }
        [DisplayName("Material")]
        public string DescricaoProduto { get; set; }
        [DisplayName("Status")]
        public int? CodigoStatusProcessoCotacao { get; set; }

    }

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

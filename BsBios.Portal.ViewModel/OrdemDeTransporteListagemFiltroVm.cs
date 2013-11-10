using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class OrdemDeTransporteListagemFiltroVm
    {
        [DisplayName("Código do Fornecedor")]
        public string CodigoDoFornecedor { get; set; }
        [DisplayName("Nome do Fornecedor")]
        public string NomeDoFornecedor { get; set; }
        [DisplayName("Nº da Ordem de Transporte")]
        public int? NumeroDaOrdemDeTransporte { get; set; }
    }
}
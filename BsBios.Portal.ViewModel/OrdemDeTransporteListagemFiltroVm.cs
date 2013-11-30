using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BsBios.Portal.ViewModel
{
    public class OrdemDeTransporteListagemFiltroVm
    {
        [DisplayName("Código da Transportadora")]
        public string CodigoDaTransportadora { get; set; }
        [DisplayName("Nome da Transportadora")]
        public string NomeDaTransportadora { get; set; }
        [DisplayName("Nome do Fornecedor da Mercadoria")]
        public string NomeDoFornecedorDaMercadoria{ get; set; }
        [DisplayName("Nº da Ordem de Transporte")]
        public int? NumeroDaOrdemDeTransporte { get; set; }
        [DisplayName("Número do Contrato")]
        public string NumeroDoContrato { get; set; }
        [DisplayName("Município de Origem")]
        public string CodigoDoMunicipioDeOrigem { get; set; }
    }
}
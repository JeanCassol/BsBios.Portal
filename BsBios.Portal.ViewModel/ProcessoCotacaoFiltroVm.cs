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
}

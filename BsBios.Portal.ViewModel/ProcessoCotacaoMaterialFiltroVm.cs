namespace BsBios.Portal.ViewModel
{
    public class ProcessoCotacaoMaterialFiltroVm
    {
        public string CodigoFornecedor { get; set; }
        //1 - material; 2 - frete
        public int TipoDeCotacao { get; set; }
        public string CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public int? CodigoStatusProcessoCotacao { get; set; }


    }
}

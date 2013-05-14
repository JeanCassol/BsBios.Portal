namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// Utilizado para salvar as cotações do Fornecedor
    /// </summary>
    public class CotacaoInformarVm: CotacaoDadosVm
    {
        public int IdProcessoCotacao { get; set; }
        public string CodigoFornecedor { get; set; }
    }
    public class CotacaoMaterialInformarVm : CotacaoMaterialDadosVm
    {
        public int IdProcessoCotacao { get; set; }
        public string CodigoFornecedor { get; set; }
    }

    public class CotacaoMaterialItemInformarVm : CotacaoMaterialItemDadosVm
    {
        public int IdProcessoCotacao { get; set; }
        public int IdProcessoCotacaoItem { get; set; }
        public int IdCotacao { get; set; }
        public CotacaoImpostosVm Impostos { get; set; }

    
    }

}

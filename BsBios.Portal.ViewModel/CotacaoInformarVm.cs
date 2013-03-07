using System.ComponentModel.DataAnnotations;

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
}

using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoMaterialItemController : Controller
    {
        //
        // GET: /CotacaoMaterialItem/
        private readonly IConsultaCotacaoDoFornecedor _consulta;

        public CotacaoMaterialItemController(IConsultaCotacaoDoFornecedor consulta)
        {
            _consulta = consulta;
        }

        public ActionResult EditarCadastro(int idProcessoCotacao, string codigoDoFornecedor, 
            string numeroDaRequisicao, string numeroDoItemDaRequisicao)
        {
            CotacaoMaterialItemCadastroVm vm = _consulta.ConsultarCotacaoDeItemDeMaterial(idProcessoCotacao, codigoDoFornecedor, numeroDaRequisicao, numeroDoItemDaRequisicao);
            return PartialView("../CotacaoMaterial/_CadastroItem",vm);
        }

    }
}

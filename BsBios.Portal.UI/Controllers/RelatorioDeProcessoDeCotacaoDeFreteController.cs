using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioDeProcessoDeCotacaoDeFreteController : Controller
    {
        private readonly IConsultaStatusProcessoCotacao _consultaStatusProcessoCotacao;

        public RelatorioDeProcessoDeCotacaoDeFreteController(IConsultaStatusProcessoCotacao consultaStatusProcessoCotacao)
        {
            _consultaStatusProcessoCotacao = consultaStatusProcessoCotacao;
        }

        public ActionResult Relatorio()
        {
            ViewBag.StatusProcessoCotacao = _consultaStatusProcessoCotacao.Listar();
            return View();
        }
    }
}

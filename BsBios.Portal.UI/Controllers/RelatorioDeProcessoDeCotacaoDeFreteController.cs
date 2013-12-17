using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioDeProcessoDeCotacaoDeFreteController : Controller
    {
        private readonly IConsultaStatusProcessoCotacao _consultaStatusProcessoCotacao;
        private readonly IConsultaEscolhaSimples _consultaEscolhaSimples;
        private readonly IConsultaSelecaoDeFornecedores _consultaSelecaoDeFornecedores;

        public RelatorioDeProcessoDeCotacaoDeFreteController(IConsultaStatusProcessoCotacao consultaStatusProcessoCotacao, IConsultaSelecaoDeFornecedores consultaSelecaoDeFornecedores, IConsultaEscolhaSimples consultaEscolhaSimples)
        {
            _consultaStatusProcessoCotacao = consultaStatusProcessoCotacao;
            _consultaSelecaoDeFornecedores = consultaSelecaoDeFornecedores;
            _consultaEscolhaSimples = consultaEscolhaSimples;
        }

        public ActionResult Relatorio()
        {
            ViewBag.StatusProcessoCotacao = _consultaStatusProcessoCotacao.Listar();
            ViewBag.SelecaoDeFornecedores = _consultaSelecaoDeFornecedores.Listar();
            ViewBag.EscolhaSimples = _consultaEscolhaSimples.Listar();

            return View();
        }
    }

}

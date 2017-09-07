using System.Web.Mvc;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioDeProcessoDeCotacaoDeFreteController : Controller
    {
        private readonly IConsultaStatusProcessoCotacao _consultaStatusProcessoCotacao;
        private readonly IConsultaEscolhaSimples _consultaEscolhaSimples;
        private readonly IConsultaSelecaoDeFornecedores _consultaSelecaoDeFornecedores;
        private readonly IConsultaTerminal _consultaTerminal;


        public RelatorioDeProcessoDeCotacaoDeFreteController(IConsultaStatusProcessoCotacao consultaStatusProcessoCotacao, IConsultaSelecaoDeFornecedores consultaSelecaoDeFornecedores, IConsultaEscolhaSimples consultaEscolhaSimples, IConsultaTerminal consultaTerminal)
        {
            _consultaStatusProcessoCotacao = consultaStatusProcessoCotacao;
            _consultaSelecaoDeFornecedores = consultaSelecaoDeFornecedores;
            _consultaEscolhaSimples = consultaEscolhaSimples;
            _consultaTerminal = consultaTerminal;
        }

        public ActionResult Relatorio()
        {
            ViewBag.StatusProcessoCotacao = _consultaStatusProcessoCotacao.Listar();
            ViewBag.SelecaoDeFornecedores = _consultaSelecaoDeFornecedores.Listar();
            ViewBag.EscolhaSimples = _consultaEscolhaSimples.Listar();
            ViewBag.Terminais = _consultaTerminal.ListarTodos();

            return View();
        }
    }

}

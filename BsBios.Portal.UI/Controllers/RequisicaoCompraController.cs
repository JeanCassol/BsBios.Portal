using System;
using System.Web.Mvc;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RequisicaoCompraController : Controller
    {
        public JsonResult Listar(PaginacaoVm paginacaoVm, SelecionarRequisicaoFiltroVm filtro)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SelecionarRequisicaoFiltroVm
    {
        public string CodigoDoGrupoDeCompras { get; set; }
        public DateTime? DataDeSolicitacaoInicial { get; set; }
        public DateTime? DataDeSolicitacaoFinal { get; set; }
    }
}
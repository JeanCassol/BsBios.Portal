using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RequisicaoDeCompraAlteracaoController : Controller
    {
        private readonly IAlteradorDeRequisicaoDeCompra _alteradorDeRequisicaoDeCompra;

        public RequisicaoDeCompraAlteracaoController(IAlteradorDeRequisicaoDeCompra alteradorDeRequisicaoDeCompra)
        {
            _alteradorDeRequisicaoDeCompra = alteradorDeRequisicaoDeCompra;
        }

        [HttpPost]
        public JsonResult Bloquear(int idRequisicaoCompra)
        {
            try
            {
                _alteradorDeRequisicaoDeCompra.Bloquear(idRequisicaoCompra);
                return Json(new {Sucesso = true},JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new{Sucesso = false, Mensagem = ex.Message},JsonRequestBehavior.AllowGet);
            }
        }
    }
}
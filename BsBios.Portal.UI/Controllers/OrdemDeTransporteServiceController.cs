using System;
using System.Web.Mvc;
using BsBios.Portal.Application.DTO;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class OrdemDeTransporteServiceController : Controller
    {

        private readonly IOrdemDeTransporteService _ordemDeTransporteService;

        public OrdemDeTransporteServiceController(IOrdemDeTransporteService ordemDeTransporteService)
        {
            _ordemDeTransporteService = ordemDeTransporteService;
        }

        [HttpPost]
        public JsonResult SalvarOrdem(OrdemDeTransporteAtualizarDTO ordemDeTransporteAtualizarVm)
        {
            try
            {
                _ordemDeTransporteService.AtualizarOrdemDeTransporte(ordemDeTransporteAtualizarVm);
                return Json(new {Sucesso = true, Mensagem = "Ordem de Transporte salva com sucesso."});

            }
            catch (Exception exception)
            {

                return Json(new {Sucesso = false, Mensagem =  ExceptionUtil.ExibeDetalhes(exception)});
            }
            
        }


        [HttpPost]
        public JsonResult SalvarColeta(ColetaSalvarVm coletaSalvarVm)
        {
            try
            {
                var quantidadeColetada = _ordemDeTransporteService.SalvarColeta(coletaSalvarVm);
                return Json(new { Sucesso = true, QuantidadeColetada =  quantidadeColetada});
            }
            catch (Exception ex)
            {

                return Json(new { Sucesso = false, Mensagem = ExceptionUtil.ExibeDetalhes(ex) });
            }


        }

    }
}

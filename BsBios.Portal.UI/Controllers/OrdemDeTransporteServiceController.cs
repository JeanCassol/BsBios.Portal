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

        [HttpPost]
        public ActionResult RealizarColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            try
            {
                _ordemDeTransporteService.RealizarColeta(idDaOrdemDeTransporte, idDaColeta);
                return Json(new {Sucesso = true});
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ExceptionUtil.ExibeDetalhes(ex)});
            }
        }

        [HttpPost]
        public ActionResult RemoverColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            try
            {
                decimal quantidadeColeta =  _ordemDeTransporteService.RemoverColeta(idDaOrdemDeTransporte, idDaColeta);
                return Json(new { Sucesso = true, QuantidadeColetada = quantidadeColeta });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ExceptionUtil.ExibeDetalhes(ex) });
            }
        }

        [HttpPost]
        public ActionResult FecharParaColeta(int idDaOrdemDeTransporte, string motivo)
        {
            try
            {
                _ordemDeTransporteService.FecharParaColeta(idDaOrdemDeTransporte, motivo);
                return Json(new { Sucesso = true});
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ExceptionUtil.ExibeDetalhes(ex) });
            }
        }


    }
}

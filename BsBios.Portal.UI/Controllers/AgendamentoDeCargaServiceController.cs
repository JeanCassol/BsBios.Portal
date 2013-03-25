using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class AgendamentoDeCargaServiceController : Controller
    {
        private readonly IAgendamentoDeCargaService _agendamentoDeCargaService;

        public AgendamentoDeCargaServiceController(IAgendamentoDeCargaService agendamentoDeCargaService)
        {
            _agendamentoDeCargaService = agendamentoDeCargaService;
        }

        [HttpPost]
        public JsonResult Excluir(int idQuota, int idAgendamento)
        {
            try
            {
                QuotaPesoVm quotaPesoVm = _agendamentoDeCargaService.ExcluirAgendamento(idQuota, idAgendamento);
                return Json(new {Sucesso = true, Quota = quotaPesoVm});
            }
            catch (Exception ex)
            {
                return  Json(new{Sucesso = false, Mensagem = ex.Message});
            }
            
        }

        [HttpPost]
        public JsonResult Editar()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public JsonResult SalvarAgendamentoDeCarregamento(AgendamentoDeCarregamentoCadastroVm agendamentoDeCarregamentoCadastroVm)
        {
            try
            {
                QuotaPesoVm quotaPesoVm = _agendamentoDeCargaService.SalvarAgendamentoDeCarregamento(agendamentoDeCarregamentoCadastroVm);
                return Json(new { Sucesso = true, Quota = quotaPesoVm});
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioAgendamentoVisualizacaoController : Controller
    {
        //
        // GET: /RelatorioAgendamentoVisualizacao/

        public ActionResult QuotaPrevistaRealizada()
        {
            var quotas = new List<QuotaCadastroVm>
                {
                    new QuotaCadastroVm
                        {
                            Data = DateTime.Today.AddDays(1).ToShortDateString(),
                            FluxoDeCarga = "Carregamento",
                            Fornecedor = "Fornecedor A",
                            Material = "Farelo",
                            Peso = 30,
                            Terminal = "1000"
                        },
                    new QuotaCadastroVm
                        {
                            Data = DateTime.Today.AddDays(2).ToShortDateString(),
                            FluxoDeCarga = "Descarregamento",
                            Fornecedor = "Fornecedor B",
                            Material = "Soja",
                            Peso = 50,
                            Terminal = "1000"
                        },
                };
            return View(quotas);
        }

    }
}

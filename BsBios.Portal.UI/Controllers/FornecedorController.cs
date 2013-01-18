using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class FornecedorController : Controller
    {
        private  readonly IList<FornecedorListagemVm> _fornecedores;
        //
        // GET: /Fornecedor/
        public FornecedorController()
        {
            _fornecedores = new List<FornecedorListagemVm>();
            _fornecedores.Add(new FornecedorListagemVm()
                {
                    Id = 1,
                    Nome = "Transporta Cometa"
                });
            _fornecedores.Add(new FornecedorListagemVm()
            {
                Id = 2,
                Nome = "Transporta Vapt Vupt"
            });
            _fornecedores.Add(new FornecedorListagemVm()
            {
                Id = 3,
                Nome = "Transporta Centenária"
            });
            _fornecedores.Add(new FornecedorListagemVm()
            {
                Id = 4,
                Nome = "Transporta Mercúrio"
            });
        }

        [HttpGet]
        public JsonResult Listar()
        {
            return Json(new { registros = _fornecedores.OrderBy(x => x.Nome), totalCount = _fornecedores.Count }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult Index()
        {
            return PartialView("_selecionarFornecedor");
        }

    }
}

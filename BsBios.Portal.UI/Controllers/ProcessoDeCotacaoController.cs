using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class ProcessoDeCotacaoController : Controller
    {

        [HttpPost]
        public JsonResult AtualizarFornecedores(AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm atualizacaoDosFornecedoresVm)
        {
            return Json(new {Sucesso = true, Mensagem = "Atualização dos Fornecedores realizada com sucesso."});
        }

    }
}

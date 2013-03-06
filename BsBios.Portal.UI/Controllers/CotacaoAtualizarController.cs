﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoAtualizarController : Controller
    {
        private readonly IAtualizadorDeCotacao _atualizadorDeCotacao;

        public CotacaoAtualizarController(IAtualizadorDeCotacao atualizadorDeCotacao)
        {
            _atualizadorDeCotacao = atualizadorDeCotacao;
        }

        [HttpPost]
        public JsonResult Salvar(CotacaoInformarVm cotacaoInformarVm)
        {
            try
            {
                _atualizadorDeCotacao.Atualizar(cotacaoInformarVm);
                return Json(new { Sucesso = cotacaoInformarVm != null });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }

        }
    }
}

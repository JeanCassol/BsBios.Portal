using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class GerenciadorUsuarioController : Controller
    {
        private readonly IGerenciadorUsuario _gerenciadorUsuario;

        public GerenciadorUsuarioController(IGerenciadorUsuario gerenciadorUsuario)
        {
            _gerenciadorUsuario = gerenciadorUsuario;
        }

        [SecurityFilter]
        [HttpPost]
        public JsonResult Ativar(string login)
        {
            try
            {
                _gerenciadorUsuario.Ativar(login);
                return Json(new {Sucesso = true});
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});
            }
        }

        [SecurityFilter]
        [HttpPost]
        public JsonResult Bloquear(string login)
        {
            try
            {
                _gerenciadorUsuario.Bloquear(login);
                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CriarSenha(string login)
        {
            try
            {
                UsuarioConsultaVm vm = _gerenciadorUsuario.CriarSenha(login);
                return Json(new { Sucesso = true, Mensagem = "Uma nova senha foi enviada para o e-mail " + vm.Email  + "." });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [SecurityFilter]
        [HttpPost]
        public JsonResult AtualizarPerfis(string login, IList<Enumeradores.Perfil> perfis)
        {
            try
            {
                _gerenciadorUsuario.AtualizarPerfis(login, perfis);
                return Json(new { Sucesso = true});
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});

            }
            
        }

        [HttpPost]
        public JsonResult AlterarSenha(AlterarSenhaVm alterarSenhaVm)
        {
            try
            {
                _gerenciadorUsuario.AlterarSenha(alterarSenhaVm.Login, alterarSenhaVm.SenhaAtual, alterarSenhaVm.SenhaNova);
                return Json(new {Sucesso = true});
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});
            }
            
        }
    }
}
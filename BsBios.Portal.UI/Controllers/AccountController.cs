using System;
using System.Web.Mvc;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    //Não pode ter SecurityFilter no controller. Colocar apenas nas Actions que for necessário
    //[SecurityFilter]
    public class AccountController:Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            if (LoginInfo.UsuarioEstaLogado)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToLocal(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("Login", new LoginVm());
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm model, string returnUrl)
        {
            try
            {
                _accountService.Login(model.Usuario, model.Senha);
                if (! string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToLocal(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ExceptionUtil.ExibeDetalhes(ex));
                return View(model);
            }


        }
        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            _accountService.Logout();

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public ActionResult EsqueciMinhaSenha(string login)
        {
            ViewBag.ReturnUrl = "";
            return View("EsqueciMinhaSenha",(object) login);
        }
        
        public ActionResult AlterarSenha(string login)
        {
            return View(new AlterarSenhaVm {Login = login});
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        #endregion

    }
}

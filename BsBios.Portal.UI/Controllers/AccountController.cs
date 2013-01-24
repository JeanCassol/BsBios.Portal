using System;
using System.Web.Mvc;
using System.Web.Security;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;
using StructureMap;
using StructureMap.Pipeline;
using WebMatrix.WebData;

namespace BsBios.Portal.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService authenticationProvider)
        {
            _accountService = authenticationProvider;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Login");
        }

        //
        // POST: /Account/Login

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    ModelState.AddModelError("", "Usuário ou senha incorreta.");
        //    return View(model);
        //}

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm model)
        {
            try
            {
                UsuarioConectado usuarioConectado = _accountService.Login(model.Usuario, model.Senha);
                Session["UsuarioConectado"] = usuarioConectado;
                ObjectFactory.Configure(c => c.For<UsuarioConectado>()
                    .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.HttpSession))
                    .Use(usuarioConectado));
                return RedirectToAction("Index", "Home");
            }
            catch (UsuarioNaoCadastradoException)
            {
                
                throw;
            }


            ModelState.AddModelError("", "Usuário ou senha incorreta.");
            return View(model);
        }
        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Account");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UsuarioVm model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.Login, model.Senha);
                    WebSecurity.Login(model.Login, model.Senha);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        //internal class ExternalLoginResult : ActionResult
        //{
        //    public ExternalLoginResult(string provider, string returnUrl)
        //    {
        //        Provider = provider;
        //        ReturnUrl = returnUrl;
        //    }

        //    public string Provider { get; private set; }
        //    public string ReturnUrl { get; private set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        //    }
        //}

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}

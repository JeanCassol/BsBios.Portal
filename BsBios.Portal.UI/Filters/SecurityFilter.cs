using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace BsBios.Portal.UI.Filters
{
    public class SecurityFilter : ActionFilterAttribute
    {

        //private bool SessaoExpirou()
        //{
        //    return HttpContext.Current.Session["UsuarioConectado"] == null;
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!filterContext.HttpContext.User.Identity.IsAuthenticated
            //    || SessaoExpirou())
            if (!LoginInfo.UsuarioEstaLogado)
            {
                string redirectUrl = "";

                if (filterContext.HttpContext.Request.UrlReferrer != null)
                {
                    string redirectOnSuccess = filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri;
                    redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                }

                string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {

                    filterContext.Result = new JsonResult()
                        {
                            Data = new
                                {
                                    SessaoExpirada = true,
                                    Mensagem = "A sessão expirou.",
                                    ReturnUrl = loginUrl
                                },
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet

                        };
                }
                else
                {
                    filterContext.Result = new RedirectResult(loginUrl);
                }
            }
            else
            {
                if (filterContext.Controller.ControllerContext.HttpContext.Session != null)
                    filterContext.Controller.ViewBag.UsuarioConectado =
                        filterContext.Controller.ControllerContext.HttpContext.Session["UsuarioConectado"];
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
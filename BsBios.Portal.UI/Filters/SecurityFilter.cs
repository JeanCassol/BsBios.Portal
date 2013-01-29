using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BsBios.Portal.UI.Filters
{
    public class SecurityFilter : ActionFilterAttribute
    {

        private bool SessaoExpirou()
        {
            //HttpContext ctx = HttpContext.Current;
            //if (ctx.Session != null)
            //{

            //    // check if a new session id was generated
            //    if (ctx.Session.IsNewSession)
            //    {

            //        // If it says it is a new session, but an existing cookie exists, then it must
            //        // have timed out
            //        string sessionCookie = ctx.Request.Headers["Cookie"];
            //        if ((null != sessionCookie) &&
            //            (sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) >= 0))
            //        {
            //            if (ctx.Request.IsAuthenticated)
            //            {
            //                FormsAuthentication.SignOut();
            //            }
            //            return true;
            //        }

            //    }
            //}

            //return false;
            return HttpContext.Current.Session["UsuarioConectado"] == null;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated
                || SessaoExpirou())
            {
                string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;
                string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                filterContext.Result = new RedirectResult(loginUrl);
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
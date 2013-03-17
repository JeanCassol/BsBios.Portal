using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BsBios.Portal.UI.Filters
{
    public static class LoginInfo
    {
        public static bool UsuarioEstaLogado
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated
                       && HttpContext.Current.Session["UsuarioConectado"] != null;
            }
        }
    }
}
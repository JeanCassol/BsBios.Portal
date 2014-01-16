using System.IO;
using System.Web.Mvc;

namespace BsBios.Portal.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                var html =  sw.GetStringBuilder().ToString();

                html = html.Replace("\n", "");
                html = html.Replace("\r", "");
                html = html.Replace("  ", "");

                return html;

            }
        }
    }
}

using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StructureMap;

namespace BsBios.Portal.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.RegisterIoc();
            //AuthConfig.RegisterAuth();
        }

        protected void Application_EndRequest()
        {
            // Make sure to dispose of NHibernate session if created on this web request
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("Iniciando Sessao");
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{

        //}
    }
}
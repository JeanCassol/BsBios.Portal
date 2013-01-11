using System.Web.Mvc;
using System.Web.Routing;

namespace BsBios.Portal.Infra.BootStrap
{
    public class RegisterRoutes : IBootstrapTask
    {
        private readonly RouteCollection _routes;

        public RegisterRoutes()
            : this(RouteTable.Routes)
        {
        }

        private RegisterRoutes(RouteCollection routes)
        {
            _routes = routes;
        }

        #region IBootstrapperTask Members

        public void Execute()
        {
            AreaRegistration.RegisterAllAreas();

            // Turns off the unnecessary file exists check
            _routes.RouteExistingFiles = true;

            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            _routes.IgnoreRoute("{*favicon}", new {favicon = @"(.*/)?favicon.ico(/.*)?"});

            // Ignore text, html, files.
            _routes.IgnoreRoute("{file}.txt");
            _routes.IgnoreRoute("{file}.htm");
            _routes.IgnoreRoute("{file}.html");

            /*_routes.MapRoute(
                "Root", // Route name
                "", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );*/


            _routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        #endregion
    }
}
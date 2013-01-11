using System.Web.Mvc;
using BsBios.Portal.ApplicationServices.Contracts;

namespace BsBios.Portal.UI.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHelloWorld _helloWorld;

        public HomeController(IHelloWorld helloWorld)
        {
            _helloWorld = helloWorld;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ContentResult SayHello()
        {
            return Content(_helloWorld.SayHello("Mauro Leal"));
        }

        public ActionResult Menu()
        {
            throw new System.NotImplementedException();
        }
    }
}

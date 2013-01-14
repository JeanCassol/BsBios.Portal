﻿using System.Web.Mvc;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.Domain.Model;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    public class HomeController : Controller
    {

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

        public ViewResult Menu()
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            return View(usuarioConectado.Perfil.Menus);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infocursos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IniciarSesion()
        {
            ViewBag.Message = "Your register page.";
            return View();
        }

        public ActionResult BuscadorAlumno()
        {
            ViewBag.Message = "Your Search alumno page.";

            return View();
        }
       
    }
}
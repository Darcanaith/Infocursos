using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infocursos.Controllers
{
    public class FormadorController : Controller
    {
        // GET: Formador
        public ActionResult FormadorPerfil()
        {
            return View();
        }
        public ActionResult PaginaFormador()
        {
            ViewBag.Message = "Your Formador page.";

            return View();
        }
       
        public ActionResult FormadorPerfilCurso()
        {
            ViewBag.Message = "Your Formador profile Course page.";

            return View();
        }
        public ActionResult FormadorPerfilInfo()
        {
            ViewBag.Message = "Your Formador profile Info page.";

            return View();
        }
        public ActionResult FormadorPerfilPublicada()
        {
            ViewBag.Message = "Your Formador profile Publicada page.";

            return View();
        }
        public ActionResult ListaAlumnos()
        {
            ViewBag.Message = "Your List Alumno page.";

            return View();
        }
    }
}
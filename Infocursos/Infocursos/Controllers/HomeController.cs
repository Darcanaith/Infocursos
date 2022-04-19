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
        public ActionResult RegistroAlumno()
        {
            ViewBag.Message = "Your register alumno page.";

            return View();
        }
        public ActionResult RegistroFormador()
        {
            ViewBag.Message = "Your register trainer page.";

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
        public ActionResult BuscadorAlumno()
        {
            ViewBag.Message = "Your Search alumno page.";

            return View();
        }
        public ActionResult PaginaFormador()
        {
            ViewBag.Message = "Your Formador page.";

            return View();
        }
        public ActionResult ListaAlumnos()
        {
            ViewBag.Message = "Your List Alumno page.";

            return View();
        }
        public ActionResult AlumnoPerfil()
        {
            ViewBag.Message = "Your Alumno profile page.";

            return View();
        }
        public ActionResult AlumnoMisCursos()
        {
            ViewBag.Message = "Your Course Alumno Info page.";

            return View();
        }
    }
}
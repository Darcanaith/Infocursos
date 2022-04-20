using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infocursos.Controllers
{
    public class AlumnoController : Controller
    {
        // GET: Alumno
        public ActionResult AlumnoPerfil()
        {
            return View();
        }
        public ActionResult AlumnoMisCursos()
        {
            ViewBag.Message = "Your Course Alumno Info page.";

            return View();
        }
    }
}
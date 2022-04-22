using Infocursos.DAL;
using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Infocursos.Models.Enums;

namespace Infocursos.Controllers
{
    public class CursoController : Controller
    {
        // GET: Curso
        public ActionResult Curso()
        {
            return View();
        }

        public ActionResult CursoBusqueda()
        {
            DAL_Provincia dal_Provincia = new DAL_Provincia();
            ViewBag.ErrorProvincias = null;

            if ((dal_Provincia.Select_Provincia(null, null)).Count == 0)
                ViewBag.ErrorProvincias = "Problemas con la conexion de BBDD.";

            if (String.IsNullOrEmpty("" + ViewBag.ErrorProvincias))
            {
                List<Provincia> provincias = new List<Provincia>();
                provincias = dal_Provincia.Select_Provincia(null, null);
                @ViewData["Provincia"] = provincias;
                return View();
            }
            return View();
        }



    }
}
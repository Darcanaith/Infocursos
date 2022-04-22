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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*
            DAL_Provincia dal_Provincia = new DAL_Provincia();

            if ((dal_Provincia.Select_Provincia(null, null)).Count == 0)
                ViewBag.ErrorProvincias = "Problemas con la conexion de BBDD.";

            if (String.IsNullOrEmpty("" + ViewBag.NombreCursoError))
            {
                List<Provincia> provincias = new List<Provincia>();
                provincias = dal_Provincia.Select_Provincia(null, null);
                @ViewData["Provincia"] = provincias;
                return View();
            }*/
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

        [HttpPost]
        public ActionResult Login()
        {
            DAL_Usuario dal_Usuario = new DAL_Usuario();
            List<Filtro> filtros = new List<Filtro>();

            ViewBag.ErrorEmail = null;
            string email = Request["email"];
            ViewBag.ErrorPassword = null;
            string password = Request["password"];


            if (String.IsNullOrEmpty(email))
                ViewBag.ErrorEmail = "Usuario Incorrecto";
            else
            {
                filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", email, ECondicionText.Igual));
                if ((dal_Usuario.Select_Usuario(filtros, null)).Count == 0)
                    ViewBag.ErrorEmail = "Usuario Incorrecto";
                else
                {
                    ViewData["LoginEmailText"] = email;
                    if (!(dal_Usuario.Select_Usuario(filtros, null)).First().Password.Equals(password))
                        ViewBag.ErrorPassword = "Contraseña Incorrecta";
                }
            }

            if (String.IsNullOrEmpty(password))
                ViewBag.ErrorPassword = "Contraseña Incorrecta";


            if (String.IsNullOrEmpty("" + ViewBag.ErrorPassword + ViewBag.ErrorEmail))
            {
                filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", email, ECondicionText.Igual));
                DAL_Alumno dal_Alumno = new DAL_Alumno();
                DAL_Formador dal_Formador = new DAL_Formador();

                if ((dal_Alumno.Select_Alumno(filtros, null)).Count() > 0)
                    Session["User"] = "Alumno/" + (dal_Alumno.Select_Alumno(filtros, null)).First().Email;
                else if ((dal_Formador.Select_Formador(filtros, null)).Count() > 0)
                    Session["User"] = "Formador/" + (dal_Formador.Select_Formador(filtros, null)).First().Email;

                return View("Index");
            }

            return View("IniciarSesion");
        }

        public ActionResult BuscadorAlumno()
        {
            ViewBag.Message = "Your Search alumno page.";

            return View();
        }

        [HttpPost]
        public ActionResult buscarCurso()
        {
            DAL_Provincia dal_Provincia = new DAL_Provincia();
            List<Filtro> filtros = new List<Filtro>();

            //DAL_Usuario dal_Usuario = new DAL_Usuario();
            //List<Filtro> filtros = new List<Filtro>();

            string busquedaCurso = Request["nombreCurso"];
            ViewBag.NombreCursoError = null;

            if (String.IsNullOrEmpty(busquedaCurso))
                ViewBag.NombreCursoError = "Busqueda incorrecta.";
            else
            {
                filtros.Add(new Filtro("Nombre_provincia", busquedaCurso, ECondicionText.Cont));
                if ((dal_Provincia.Select_Provincia(filtros, null)).Count == 0)
                    ViewBag.NombreCursoError = "No se han encontrado cursos con esta informacion.";
            }
            if (String.IsNullOrEmpty("" + ViewBag.NombreCursoError))
            {
                List<Provincia> provincias = new List<Provincia>();
                provincias = dal_Provincia.Select_Provincia(filtros, null);
                @ViewData["Provincia"] = provincias;
                return View();
            }
            return View();

        }

    }
}
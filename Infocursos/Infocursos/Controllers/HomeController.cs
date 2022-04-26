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

            DAL_Provincia dal_Provincia = new DAL_Provincia();

            if ((dal_Provincia.Select_Provincia(null, null)).Count == 0)
                ViewBag.ErrorProvincias = "Problemas con la conexion de BBDD.";

            if (String.IsNullOrEmpty("" + ViewBag.NombreCursoError))
            {
                List<Provincia> provincias = new List<Provincia>();
                provincias = dal_Provincia.Select_Provincia(null, null);
                @ViewData["Provincia"] = provincias;
                return View();
            }
            return View();
        }
        public ActionResult CerrarSesion()
        {
            Session["User"]=null;
            return View("../Home/Index");
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
                    Session["User"] = (dal_Alumno.Select_Alumno(filtros, null)).First();
                else if ((dal_Formador.Select_Formador(filtros, null)).Count() > 0)
                    Session["User"] = (dal_Formador.Select_Formador(filtros, null)).First();

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("IniciarSesion", "Home");
        }

        public ActionResult BuscadorAlumno()
        {
            ViewBag.Message = "Your Search alumno page.";

            return View();
        }

        [HttpPost]
        public ActionResult BuscarCurso()
        {
            //Inicializo los recursos necesarios.
            DAL_Curso dal_curso = new DAL_Curso();
            DAL_Provincia dal_provincia = new DAL_Provincia();


            List<Curso> cursos = new List<Curso>();
            List<Provincia> provincias = new List<Provincia>();

            List<Filtro> filtrosCurso = new List<Filtro>();
            List<Filtro> filtrosProvincia = new List<Filtro>();
            
            

            //Recojo las variables que necesitaremos.
            string busquedaCurso = Request["nombreCurso"];
            string provincia = Request["provincia"];

            //Busco si hay resultados segun el filtro aplicado
            if (provincia != null && provincia != "0")
            {
                //Creo los filtros de municipio necesarios.
                filtrosProvincia.Add(new Filtro("Nombre_provincia", provincia, ECondicionText.Igual));
            }
            else if (busquedaCurso != null && busquedaCurso != "")
            {
                //Creo los filtros de curso necesarios..
                filtrosCurso.Add(new Filtro("Curso_Nombre", busquedaCurso, ECondicionText.Cont));
            }



            if (filtrosProvincia.Count > 0 && filtrosCurso.Count > 0)
            {
                //Caso de que hayan ambos filtros
                if (dal_provincia.Select_Provincia(filtrosProvincia, null).Count > 0)
                {
                    provincias = dal_provincia.Select_Provincia(filtrosProvincia, null);
                    cursos = BuscarCursosByProvincia(provincias,filtrosCurso);

                    Session["Cursos"] = cursos;
                    return RedirectToAction("CursoBusqueda", "Curso");
                }
            }
            else if (filtrosCurso.Count > 0)
            {
                //Casos de que solo haya filtro de nombreCurso
                if (dal_curso.Select_Curso(filtrosCurso, null).Count > 0)
                {
                    cursos = dal_curso.Select_Curso(filtrosCurso, null);
                    Session["Cursos"] = cursos;
                    return RedirectToAction("CursoBusqueda", "Curso");
                }
            }
            else if (filtrosProvincia.Count > 0)
            {
                //Casos de que solo haya filtro de provincia.
                if (dal_provincia.Select_Provincia(filtrosProvincia, null).Count > 0)
                {
                    provincias = dal_provincia.Select_Provincia(filtrosProvincia, null);
                    cursos = BuscarCursosByProvincia(provincias, filtrosCurso);

                    Session["Cursos"] = cursos;
                    return RedirectToAction("CursoBusqueda", "Curso");
                }
            }

            cursos = dal_curso.Select_Curso(null, null);
            Session["Cursos"] = cursos;
            return RedirectToAction("CursoBusqueda", "Curso");
        }

        private List<Curso> BuscarCursosByProvincia(List<Provincia> provincias, List<Filtro> filtrosCurso)
        {
            DAL_Curso dal_curso = new DAL_Curso();
            DAL_Municipio dAL_Municipio = new DAL_Municipio();
            DAL_Centro dal_centro = new DAL_Centro();

            List<Filtro> filtrosMunicipio = new List<Filtro>();
            List<Filtro> filtrosCentro = new List<Filtro>();


            List<Municipio> municipios = new List<Municipio>();
            List<Centro> centros = new List<Centro>();
            List<Curso> cursos = new List<Curso>();

            foreach (Provincia prov in provincias)
                filtrosMunicipio.Add(new Filtro("RId_Provincia", prov.Id_provincia.ToString(), ECondicionNum.Ig));

            municipios = dAL_Municipio.Select_Municipio(filtrosMunicipio, null);

            foreach (Municipio mun in municipios)
                filtrosCentro.Add(new Filtro("RId_Municipio", mun.Id_municipio.ToString(), ECondicionNum.Ig, EConector.OR));

            centros = dal_centro.Select_Centro(filtrosCentro, null);

            foreach (Centro cen in centros)
                filtrosCurso.Add(new Filtro("RId_Centro", cen.Id_centro.ToString(), ECondicionNum.Ig, EConector.OR));

            cursos = dal_curso.Select_Curso(filtrosCurso, null);

            return cursos;
        }
    }
}
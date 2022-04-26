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
        /// <summary>
        /// Funcion de mostrara un curso segun la id asignada.
        /// </summary>
        /// <param name="IdCurso">Filtrara los datos a buscar.</param>
        /// <returns>Vista Curso con los datos del asignado.</returns>
        public ActionResult Curso(int? IdCurso)
        {
            if (IdCurso == null)
                return View("../Home/Index");

            DAL_Curso dal_Curso = new DAL_Curso();
            List<Filtro> filtros = new List<Filtro>();
            filtros.Add(new Filtro("Id_Curso", IdCurso.ToString(), ECondicionNum.Ig));
            @ViewData["Curso"] = dal_Curso.Select_Curso(filtros, null).First();


            return View();
        }

        /// <summary>
        /// Funcion que se lanzara al buscar un curso, buscara todas las provincias dentro de nuestra BBDD.
        /// </summary>
        /// <returns>Retorna la vista de CursoBusqueda con todas las provincias de nuestra BBDD.</returns>
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
        
        /// <summary>
        /// Funcion que filtrara los cursos de nuestra base de datos segun los parametros provincia y nombre curso.
        /// </summary>
        /// <returns>Vista de busqueda de cursos con el filtrado aplicado.</returns>
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
                    cursos = BuscarCursosByProvincia(provincias, filtrosCurso);

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

        /// <summary>
        /// Funcion que sera llamada a la hora de buscar por una provincia en concreto.
        /// </summary>
        /// <param name="provincias">La provincia en la cual se filtrara.</param>
        /// <param name="filtrosCurso">Filtros que se han de aplicar a la hora de buscar el curso.</param>
        /// <returns>Un listado de cursos filtrados por los parametros.</returns>
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
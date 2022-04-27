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
    public class AlumnoController : Controller
    {
    /// <summary>
    /// Funcion que comprobara que el usuario alumno este con sesion iniciada.
     /// </summary>
     /// <returns>False en caso de que no este iniciado;True en caso de que si.</returns>
        public bool AlumnoIsLoged()
        {
            if (Session["User"] == null || ((Usuario)Session["User"] as Alumno) == null)
                return false;
            else
                return true;
        }
        // GET: Alumno
        /// <summary>
        /// Funcion que mostrara el perfil del alumno segun la id asignada.
        /// </summary>
        /// <param name="IdAlumno">Filtrara los campos dentro de nuestra BBDD.</param>
        /// <returns>Vista del alumno perfil con sus datos.</returns>

        public ActionResult AlumnoPerfil(int? IdAlumno)
        {
            DAL_Alumno dal_Alumno = new DAL_Alumno();
            List<Filtro> filtros = new List<Filtro>();
            filtros.Add(new Filtro("Id_User", IdAlumno.ToString(), ECondicionNum.Ig));

            if (IdAlumno == null)
                if (!AlumnoIsLoged())
                    return View("../Home/Index");
                else
                {
                    Session["AlumnoAMostrar"] = (Alumno)Session["User"];
                    Session["Alumno_ShowEditOptions"] = "normal";
                    if (((Alumno)Session["User"]).User_Descripcion == null)
                        Session["Alumno_ShowEditOptionsDescripccion"] = "none";
                    else
                        Session["Alumno_ShowEditOptionsDescripccion"] = "normal";
                }
            else
            {
                Session["AlumnoAMostrar"] = dal_Alumno.Select_Alumno(filtros, null).First();
                Session["Alumno_ShowEditOptions"] = "none";
            }

            Session["Alumno_View_Info_Cursos"] = "Info";
            Session["Alumno_ShowAddDescription"] = "EsconderAnadir";
            @ViewData["Alumno_DisplayAddButton"] = "normal";
            @ViewData["Alumno_DisplayAddDescripcion"] = "none";

            return RellenarAlumnoPerfil();
        }

        /// <summary>
        /// La siguiente funcion realizara la tarea de rellenar todo el perfil de alumno con datos recibidos desde la BBDD.
        /// </summary>
        /// <returns>Una vista rellenada con los datos del alumno actual.</returns>

        [HttpPost]
        public ActionResult RellenarAlumnoPerfil()
        {
            Alumno alumno = (Alumno)Session["AlumnoAMostrar"];


            if (!Session["Alumno_View_Info_Cursos"].Equals("Info"))
            {
                @ViewData["Alumno_DisplayInfo"] = "none";
                @ViewData["Alumno_DisplayCursos"] = "normal";
                @ViewData["Alumno_ActiveCursos"] = "active disabled";
            }
            else
            {
                @ViewData["Alumno_DisplayInfo"] = "normal";
                @ViewData["Alumno_DisplayCursos"] = "none";
                @ViewData["Alumno_ActiveInfo"] = "active disabled";
            }

            if (Session["Alumno_ShowAddDescription"].Equals("VerAnadir"))
            {
                @ViewData["Alumno_DisplayAddButton"] = "none";
                @ViewData["Alumno_DisplayAddDescripcion"] = "normal";
                if (Session["Alumno_ShowEditOptionsDescripccion"].Equals("normal"))
                {
                    Session["Alumno_ShowEditOptionsDescripccion"] = "none";
                    @ViewData["Alumno_DescripcionActual"] = alumno.User_Descripcion;
                }

            }
            else
            {
                @ViewData["Alumno_DisplayAddButton"] = "normal";
                @ViewData["Alumno_DisplayAddDescripcion"] = "none";

                if (Session["Alumno_ShowEditOptionsDescripccion"].Equals("none") && alumno.User_Descripcion != null)
                {
                    Session["Alumno_ShowEditOptionsDescripccion"] = "normal";
                }
            }

            @ViewData["Alumno_Nombre"] = alumno.User_Nombre;
            @ViewData["Alumno_Apellidos"] = alumno.User_Apellidos;
            @ViewData["Alumno_FechaNac"] = alumno.Alumno_FechaNac;
            @ViewData["Alumno_Email"] = alumno.Email;
            @ViewData["Alumno_Telefonos"] = alumno.Telefonos;
            @ViewData["Alumno_IMG_Perfil"] = alumno.IMG_Perfil;
            @ViewData["Alumno_Resumen"] = alumno.User_Resumen;
            if (alumno.Alumno_Direccion != null)
                @ViewData["Alumno_Direccion"] = alumno.Alumno_Direccion + ",";
            if (alumno.Municipio != null)
            {
                @ViewData["Alumno_Provincia"] = alumno.Municipio.Provincia.Nombre_provincia;
                @ViewData["Alumno_Municipio"] = alumno.Municipio.Nombre_municipio;
            }
            @ViewData["Alumno_Descripcion"] = alumno.User_Descripcion;
            @ViewData["Alumno_Categorias"] = alumno.Categorias;
            @ViewData["IdomaYNivel"] = alumno.Idioma_Nivel;
            @ViewData["Cursos"] = alumno.Cursos_Estado;

            DAL_Estado_Curso dal_Estado_Curso = new DAL_Estado_Curso();
            ViewData["Estados"] = dal_Estado_Curso.Select_Estado_Curso(null, null);

            return View("AlumnoPerfil");
        }

        /// <summary>
        /// Funcion que guardara en nuestra BBDD en caso de estar validada, una nueva descripcion.
        /// </summary>
        /// <returns>Una nueva vista de Alumno perfil</returns>
        public ActionResult GuardarDescripcion()
        {
            var description = Request["DescriptionTextArea"];
            if (String.IsNullOrEmpty(description) || String.IsNullOrWhiteSpace(description))
            {
                ViewBag.ErrorDescription = "*No puede guardarse una descripcion vacia";
                @ViewData["Alumno_DisplayAddButton"] = "none";
                @ViewData["Alumno_DisplayAddDescripcion"] = "normal";
            }
            else
            {
                DAL_Alumno dal_Alumno = new DAL_Alumno();

                Alumno alumno = (Alumno)Session["User"];
                alumno.User_Descripcion = description;
                dal_Alumno.Update_Alumno(alumno);
                Session["Alumno_ShowEditOptionsDescripccion"] = "normal";
                Session["Alumno_ShowAddDescription"] = "EsconderAnadir";
            }

            return RellenarAlumnoPerfil();
        }

        /// <summary>
        /// Funcion que mostrara el div de descripcion cuando alumno clicke.
        /// </summary>
        /// <param name="verOcancelar">Determinara si el alumno quiere ver o cancelar el div de descripcion.</param>
        /// <returns>Una nueva vista de Alumno perfil con los parametros de verOcancelar</returns>

        public ActionResult Descripcion_VerAnadir_Cancelar(string verOcancelar)
        {
            Session["Alumno_ShowAddDescription"] = verOcancelar;

            return RellenarAlumnoPerfil();
        }
        /// <summary>
        /// Funcion que cambiara los divs segun lo que seleccione el usuario.
        /// </summary>
        /// <param name="requestedView">Vista que se quiera ver segun el clickado.</param>
        /// <returns>Una nueva vista de Alumno perfil con los parametros de requestView.</returns>

        public ActionResult Info_Cursos(string requestedView)
        {
            Session["Alumno_View_Info_Cursos"] = requestedView;

            return RellenarAlumnoPerfil();
        }

        /// <summary>
        /// Funcion que retornara la vista de AlumnoCursos.
        /// </summary>
        /// <returns>
        /// Vista de RegistroAlumno.
        /// </returns>
        public ActionResult AlumnoMisCursos()
        {
            if (!AlumnoIsLoged())
                return View("../Home/Index");
            else
            {
                Session["AlumnoCursos"] = (Alumno)Session["User"];
                Session["Cursos_Mostrando"] = "Insctripciones";
            }

            return RellenarAlumnoMisCursos();
        }
        [HttpPost]
        public ActionResult RellenarAlumnoMisCursos()
        {
            Alumno alumno = (Alumno)Session["AlumnoCursos"];
            List<Object[]> cursosAMostrar = new List<object[]>();

            if (Session["Cursos_Mostrando"].Equals("Insctripciones"))
            {
                @ViewData["Cursos_ActiveInscripciones"] = "active disabled";
                foreach (Object[] curso_Estado in alumno.Cursos_Estado)
                    if (((Estado_Curso)curso_Estado[1]).Id_estado_curso > 0 && DateTime.Compare(((Curso)curso_Estado[0]).Fecha_final, DateTime.Now) > 0)
                        cursosAMostrar.Add(curso_Estado);
            }
            else if (Session["Cursos_Mostrando"].Equals("Guardados"))
            {
                @ViewData["Cursos_ActiveGuardados"] = "active disabled";
                foreach (Object[] curso_Estado in alumno.Cursos_Estado)
                    if (((Estado_Curso)curso_Estado[1]).Id_estado_curso == 0)
                        cursosAMostrar.Add(curso_Estado);
            }
            else if (Session["Cursos_Mostrando"].Equals("Expirados"))
            {
                @ViewData["Cursos_ActiveExpirados"] = "active disabled";
                foreach (Object[] curso_Estado in alumno.Cursos_Estado)
                    if (((Estado_Curso)curso_Estado[1]).Id_estado_curso == -1 || DateTime.Compare(((Curso)curso_Estado[0]).Fecha_final, DateTime.Now) < 0)
                        cursosAMostrar.Add(curso_Estado);
            }

            ViewData["Cursos"] = cursosAMostrar;
            DAL_Estado_Curso dal_Estado_Curso = new DAL_Estado_Curso();
            ViewData["Estados"] = dal_Estado_Curso.Select_Estado_Curso(null, null);

            return View("AlumnoMisCursos");
        }

        public ActionResult CambiarCursosMostrando(string cursosMostrando)
        {
            Session["Cursos_Mostrando"] = cursosMostrando;

            return RellenarAlumnoMisCursos();
        }

        /// <summary>
        /// Funcion que retornara la vista de RegistroAlumno
        /// </summary>
        /// <returns>Vista de RegistroAlumno</returns>

        public ActionResult RegistroAlumno()
        {
            return View();
        }

        /// <summary>
        /// Funcion que validara y dara de alta a un usuario Alumno en nuestra BBDD.
        /// </summary>
        /// <returns>
        /// Vista de inicio de sesion en caso de exito.
        /// </returns>

        [HttpPost]
        public ActionResult RegistrarAlumno()
        {
            DAL_Usuario dal_Usuario = new DAL_Usuario();
            List<Filtro> filtros = new List<Filtro>();

            ViewBag.ErrorEmail = null;
            string email = Request["email"];
            ViewBag.ErrorPassword = null;
            string password = Request["password"];
            ViewBag.ErrorPassword_repetido = null;
            string password_repetido = Request["password_repetido"];
            ViewBag.ErrorNombre = null;
            string nombre = Request["nombre"];
            ViewBag.ErrorApellido = null;
            string apellido = Request["apellido"];

            if (String.IsNullOrEmpty(email))
                ViewBag.ErrorEmail = "*Este campo es oblgatorio";
            else
            {
                filtros.Add(new Filtro("Email", email, ECondicionText.Igual));
                if ((dal_Usuario.Select_Usuario(filtros, null)).Count > 0)
                    ViewBag.ErrorEmail = "Ya existe un usuario con este Email";
                else
                    ViewData["EmailText"] = email;
            }

            if (String.IsNullOrEmpty(password))
                ViewBag.ErrorPassword = "*Este campo es oblgatorio";
            else
                ViewData["PasswordText"] = password;

            if (String.IsNullOrEmpty(password_repetido))
                ViewBag.ErrorPassword_repetido = "*Este campo es oblgatorio";
            else if (!password.Equals(password_repetido))
                ViewBag.ErrorPassword_repetido = "Debe ser igual que la contraseña";
            else
                ViewData["Password_RepetidoText"] = password_repetido;

            if (String.IsNullOrEmpty(nombre))
                ViewBag.ErrorNombre = "*Este campo es oblgatorio";
            else
                ViewData["NombreText"] = nombre;

            if (String.IsNullOrEmpty(apellido))
                ViewBag.ErrorApellido = "*Este campo es oblgatorio";
            else
                ViewData["ApellidoText"] = apellido;

            if (String.IsNullOrEmpty("" + ViewBag.ErrorApellido + ViewBag.ErrorNombre + ViewBag.ErrorPassword_repetido + ViewBag.ErrorPassword + ViewBag.ErrorEmail))
            {
                DAL_Alumno dal_Alumno = new DAL_Alumno();
                Alumno newAlumno = new Alumno(email, password, nombre, apellido);
                dal_Alumno.Insert_Alumno(newAlumno);
                return View("../Home/IniciarSesion");
            }

            return View("RegistroAlumno");
        }

        /// <summary>
        /// Funcion que sera llamada cada vez que se quiera acceder a la vista RegistroFormador.
        /// </summary>
        /// <returns>
        /// La vista RegistroFormador.
        /// </returns>

        [HttpPost]
        public ActionResult CambioARegistroFormador()
        {
            return View("../Formador/RegistroFormador");
        }
    }
}
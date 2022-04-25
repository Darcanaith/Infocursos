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
        public bool AlumnoIsLoged()
        {
            if (Session["User"] == null || ((Usuario)Session["User"] as Alumno) == null)
                return false;
            else
                return true;
        }
        // GET: Alumno
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
            if(alumno.Alumno_Direccion!=null)
                @ViewData["Alumno_Direccion"] = alumno.Alumno_Direccion+",";
            if (alumno.Municipio != null)
            {
                @ViewData["Alumno_Provincia"] = alumno.Municipio.Provincia.Nombre_provincia;
                @ViewData["Alumno_Municipio"] = alumno.Municipio.Nombre_municipio;
            }
            @ViewData["Alumno_Descripcion"] = alumno.User_Descripcion;
            @ViewData["Alumno_Categorias"] = alumno.Categorias;
            @ViewData["IdomaYNivel"] = alumno.Idioma_Nivel;
            ViewData["Cursos"] = alumno.Cursos_Estado;

            return View("AlumnoPerfil");
        }

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

        public ActionResult Descripcion_VerAnadir_Cancelar(string verOcancelar)
        {
            Session["Alumno_ShowAddDescription"] = verOcancelar;

            return RellenarAlumnoPerfil();
        }
        public ActionResult Info_Cursos(string requestedView)
        {
            Session["Alumno_View_Info_Cursos"] = requestedView;

            return RellenarAlumnoPerfil();
        }
        public ActionResult AlumnoMisCursos()
        {
            ViewBag.Message = "Your Course Alumno Info page.";

            return View();
        }

        public ActionResult RegistroAlumno()
        {
            return View();
        }

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

        [HttpPost]
        public ActionResult CambioARegistroFormador()
        {
            return View("../Formador/RegistroFormador");
        }
    }
}
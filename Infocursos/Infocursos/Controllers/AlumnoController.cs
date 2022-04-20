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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infocursos.Models;
using Infocursos.DAL;
using static Infocursos.Models.Enums;

namespace Infocursos.Controllers
{
    public class FormadorController : Controller
    {
        public ActionResult registroFormador()
        {
            return View();
        }

        public ActionResult FormadorEditarPerfil()
        {
            return View();
        }

        // POST: Formador/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
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
                ViewBag.ErrorEntidad = null;
                string entidad = Request["Entidad"];

                if (String.IsNullOrEmpty(email))
                    ViewBag.ErrorEmail = "*Este campo es oblgatorio";
                else
                {
                    filtros.Add(new Filtro("Email", email, ECondicionText.Igual));
                    List<Usuario> usuarios = new List<Usuario>();
                    usuarios = dal_Usuario.Select_Usuario(filtros, null);
                    if (usuarios.Count > 0)
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

                if (String.IsNullOrEmpty(entidad))
                    ViewBag.ErrorApellido = "*Este campo es oblgatorio";
                else
                    ViewData["EntidadText"] = entidad;

                if (String.IsNullOrEmpty("" + ViewBag.ErrorApellido + ViewBag.ErrorNombre + ViewBag.ErrorPassword_repetido + ViewBag.ErrorPassword + ViewBag.ErrorEmail + ViewBag.ErrorEntidad))
                {
                    Formador formador = new Formador(email, password, nombre, apellido, entidad);
                    DAL_Formador dal_formador = new DAL_Formador();
                    dal_formador.Insert_Formador(formador);
                }
            }
            catch
            {
                ViewBag.Message = "Algo ha salido mal";
                return View("registroFormador");
            }
            return RedirectToAction("../Home/IniciarSesion");
        }
        // GET: Formador
        public ActionResult FormadorPerfil()
        {
            if (Session["User"] == null || (Session["User"].ToString().Split('/')[0]).Equals("Alumno"))
                return View("../Home/Index");
            List<Filtro> filtros;

            DAL_Formador dal_Formador = new DAL_Formador();
            filtros = new List<Filtro>();
            filtros.Add(new Filtro("Email", (Session["User"].ToString().Split('/')[1]), ECondicionText.Igual));
            Formador formador = dal_Formador.Select_Formador(filtros, null).First();
            @ViewData["Nombre_Entidad"] = formador.Nombre_Entidad;
            @ViewData["Resumen"] = formador.User_Resumen;
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
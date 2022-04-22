using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infocursos.Models;
using Infocursos.DAL;
using static Infocursos.Models.Enums;
using System.IO;

namespace Infocursos.Controllers
{
    public class FormadorController : Controller
    {
        public ActionResult RegistroFormador()
        {
            return View();
        }

        public ActionResult FormadorEditarPerfil()
        {
            if (Session["User"] == null || (Session["User"].ToString().Split('/')[0]).Equals("Alumno"))
                return View("../Home/Index");
            else
            {
                DAL_Formador dal_Formador = new DAL_Formador();
                List<Filtro> filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", (Session["User"].ToString().Split('/')[1]), ECondicionText.Igual));
                Formador formador = dal_Formador.Select_Formador(filtros, null).First();
                @ViewData["Nombre_Entidad"] = formador.Nombre_Entidad;
                @ViewData["User_nombre"] = formador.User_Nombre;
                @ViewData["User_apellidos"] = formador.User_Apellidos;
                @ViewData["Resumen"] = formador.User_Resumen;
                return View();
            }
            
        }

        // POST: Formador/Create
        [HttpPost]
        public ActionResult Create()
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
            string entidad = Request["entidad"];

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
                ViewBag.ErrorEntidad = "*Este campo es oblgatorio";
            else
                ViewData["EntidadText"] = entidad;

            if (String.IsNullOrEmpty("" + ViewBag.ErrorApellido + ViewBag.ErrorNombre + ViewBag.ErrorPassword_repetido + ViewBag.ErrorPassword + ViewBag.ErrorEmail + ViewBag.ErrorEntidad))
            {
                Formador formador = new Formador(email, password, nombre, apellido, entidad);
                DAL_Formador dal_formador = new DAL_Formador();
                dal_formador.Insert_Formador(formador);

                return RedirectToAction("../Home/IniciarSesion");
            }

            return View("RegistroFormador");
        }

        public ActionResult CambioARegistroAlumnor()
        {
            return View("../Alumno/RegistroAlumno");
        }

        
        public ActionResult FormadorPerfil()
        {
            @ViewData["DisplayAddButton"] = "normal";
            @ViewData["DisplayAddDescripcion"] = "none";
            if (Session["User"] == null || (Session["User"].ToString().Split('/')[0]).Equals("Alumno"))
                return View("../Home/Index");

            
            return RellenarFormadorPerfil();
        }
        public void SetButtonsInfo_Curso(bool infoIsSelected)
        {
            if (!infoIsSelected)
            {
                @ViewData["DisplayInfo"] = "none";
                @ViewData["DisplayCursos"] = "normal";
                @ViewData["BTNInfo"] = "btn-outline-primary";
                @ViewData["BTNCursos"] = "btn-primary disabled";
            }
            else
            {
                @ViewData["DisplayInfo"] = "normal";
                @ViewData["DisplayCursos"] = "none";
                @ViewData["BTNInfo"] = "btn-primary disabled";
                @ViewData["BTNCursos"] = "btn-outline-primary";
            }
        }

        [HttpPost]
        public ActionResult Info_Cursos()
        {
            string IsVisible = Request["WhoIsVisible"];
            if(IsVisible.Equals("Info"))
                @ViewData["Mostrar"] = "Curso";
            else
                @ViewData["Mostrar"] = "Info";


            return RellenarFormadorPerfil();
        }

        [HttpPost]
        public ActionResult RellenarFormadorPerfil()
        {
            if (@ViewData["Mostrar"] == null)
                @ViewData["Mostrar"] = "Info";
            SetButtonsInfo_Curso(@ViewData["Mostrar"].Equals("Info"));

            DAL_Formador dal_Formador = new DAL_Formador();
            List<Filtro> filtros = new List<Filtro>();
            filtros.Add(new Filtro("Email", (Session["User"].ToString().Split('/')[1]), ECondicionText.Igual));
            Formador formador = dal_Formador.Select_Formador(filtros, null).First();
            ViewData["IMG_Perfil"] = formador.IMG_Perfil;
            @ViewData["Nombre_Entidad"] = formador.Nombre_Entidad;
            @ViewData["Resumen"] = formador.User_Resumen;

            @ViewData["Descripcion"] = formador.User_Descripcion;

            DAL_Curso dal_Curso = new DAL_Curso();
            List<Filtro> filtrosCurso = new List<Filtro>();
            filtros.Add(new Filtro("RId_Formador",formador.Id_User.ToString(), ECondicionNum.Ig));
            List<Curso> cursos = dal_Curso.Select_Curso(filtrosCurso, null);
            List<string> horarios = new List<string>();
            List<string> modalidades = new List<string>();
            List<string> categorias = new List<string>();

            foreach (Curso curso in cursos)
            {
                horarios.Add(curso.Horario.Tipo_horario);
                modalidades.Add(curso.Modalidad.Tipo_modalidad);
                foreach (Categoria cat in curso.Categorias)
                    categorias.Add(cat.Categoria_nombre);
            }
            horarios = horarios.Distinct().ToList();
            modalidades = modalidades.Distinct().ToList();
            categorias = categorias.Distinct().ToList();

            @ViewData["Horarios"] = horarios;
            @ViewData["Modalidades"] = modalidades;
            @ViewData["Categorias"] = categorias;

            return View("FormadorPerfil");
        }

        [HttpPost]
        public ActionResult UpdateInfoFormadorPerfil(HttpPostedFileBase Imagen)
        {
            DAL_Formador dal_Formador = new DAL_Formador();
            List<Filtro> filtros = new List<Filtro>();
            filtros.Add(new Filtro("Email", (Session["User"].ToString().Split('/')[1]), ECondicionText.Igual));
            Formador formador = dal_Formador.Select_Formador(filtros, null).First();
            formador.Nombre_Entidad = Request["Entidad"];
            formador.User_Nombre = Request["Nombre"];
            formador.User_Apellidos = Request["Apellidos"];
            formador.User_Resumen = Request["Resumen"];

            if (Imagen != null)
            {
                string path = Path.Combine(Server.MapPath("~/ImagenPerfilFormador"), Path.GetFileName(Imagen.FileName));
                Imagen.SaveAs(path);
                formador.IMG_Perfil = Imagen.FileName;
            }
            ViewBag.FileStatus = "File uploaded successfully.";
            dal_Formador.Update_Formador(formador);

            
            return RellenarFormadorPerfil();
        }
        [HttpPost]
        public ActionResult GuardarDescripcion()
        {
            var description = Request["DescriptionTextArea"];
            if (String.IsNullOrEmpty(description) || String.IsNullOrWhiteSpace(description))
            {
                ViewBag.ErrorDescription = "*No puede guardarse una descripcion vacia";
                @ViewData["DisplayAddButton"] = "none";
                @ViewData["DisplayAddDescripcion"] = "normal";
            }
            else
            {
                DAL_Formador dal_Formador = new DAL_Formador();
                List<Filtro> filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", (Session["User"].ToString().Split('/')[1]), ECondicionText.Igual));
                Formador formador = dal_Formador.Select_Formador(filtros, null).First();
                formador.User_Descripcion = description;
                dal_Formador.Update_Formador(formador);
            }

            return RellenarFormadorPerfil();
        }

        [HttpPost]
        public ActionResult Descripcion_VerAnadir()
        {
            string IsVisible = Request["IsVisible"];
            if (IsVisible.Equals("normal"))
            {
                @ViewData["DisplayAddButton"] = "none";
                @ViewData["DisplayAddDescripcion"] = "normal";
            }
            else
            {
                @ViewData["DisplayAddButton"] = "normal";
                @ViewData["DisplayAddDescripcion"] = "none";
            }

            return RellenarFormadorPerfil();
        }

        [HttpPost]
        public ActionResult Descripcion_Cancelar()
        {
            string IsVisible = Request["IsVisible"];
            if (IsVisible.Equals("normal"))
            {
                @ViewData["DisplayAddButton"] = "normal";
                @ViewData["DisplayAddDescripcion"] = "none";
            }
            else
            {
                @ViewData["DisplayAddButton"] = "none";
                @ViewData["DisplayAddDescripcion"] = "normal";
            }

            return RellenarFormadorPerfil();
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
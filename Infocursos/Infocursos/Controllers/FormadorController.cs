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
        public bool FormadorIsLoged()
        {
            if (Session["User"] == null || ((Usuario)Session["User"] as Formador) == null)
                return false;
            else
                return true;
        }

        public ActionResult RegistroFormador()
        {
            return View();
        }

        public ActionResult FormadorEditarPerfil()
        {
            if (!FormadorIsLoged())
                return View("../Home/Index");

            Formador formador = (Formador)Session["User"];
            @ViewData["Nombre_Entidad"] = formador.Nombre_Entidad;
            @ViewData["User_nombre"] = formador.User_Nombre;
            @ViewData["User_apellidos"] = formador.User_Apellidos;
            @ViewData["Resumen"] = formador.User_Resumen;
            return View();

            
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
            if (!FormadorIsLoged())
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
            Formador formador = (Formador)Session["User"];

            if (@ViewData["Mostrar"] == null)
                @ViewData["Mostrar"] = "Info";
            SetButtonsInfo_Curso(@ViewData["Mostrar"].Equals("Info"));

            List<Filtro> filtros = new List<Filtro>();
            List<Filtro> filtrosCurso = new List<Filtro>();

            ViewData["IMG_Perfil"] = formador.IMG_Perfil;
            @ViewData["Nombre_Entidad"] = formador.Nombre_Entidad;
            @ViewData["Resumen"] = formador.User_Resumen;

            @ViewData["Descripcion"] = formador.User_Descripcion;

            DAL_Curso dal_Curso = new DAL_Curso();
            
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
            Formador formador = (Formador)Session["User"];
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

            DAL_Formador dal_Formador = new DAL_Formador();
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
               
                Formador formador = (Formador)Session["User"];
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
            DAL_Formador dal_Formador = new DAL_Formador();
            List<Filtro> filtros = new List<Filtro>();
            filtros.Add(new Filtro("Email", (Session["User"].ToString().Split('/')[1]), ECondicionText.Igual));
            Formador formador = dal_Formador.Select_Formador(filtros, null).First();
            DAL_Centro dal_centro = new DAL_Centro();
            DAL_Horario dal_horario = new DAL_Horario();
            DAL_Modalidad dal_modalidad = new DAL_Modalidad();

            List<Centro> centros = dal_centro.Select_Centro(filtros, null);
            List<Horario> horarios = dal_horario.Select_Horarios(null, null);
            List<Modalidad> modalidades = dal_modalidad.Select_Modalidades(null, null);

            List<string> centros_string = new List<string>();
            List<string> horarios_string = new List<string>();
            List<string> modalidades_string = new List<string>();

            foreach (Centro centro in centros)
                centros_string.Add(centro.Centro_direccion);
            foreach (Horario horario in horarios)
                horarios_string.Add(horario.Tipo_horario);
            foreach (Modalidad modalidad in modalidades)
                modalidades_string.Add(modalidad.Tipo_modalidad);

            @ViewData["centros"] = centros_string;
            @ViewData["horarios"] = horarios_string;
            @ViewData["modalidades"] = modalidades_string;

            return View();
        }


        [HttpPost]
        public ActionResult InsertFormadorCurso()
        {
            ViewBag.ErrorCursoNombre = null;
            string curso_nombre = Request["curso_nombre"];
            ViewBag.ErrorNumPlaza = null;
            string num_plaza = Request["num_plaza"];
            ViewBag.ErrorHorasTotales = null;
            string horas_totales = Request["horas_totales"];
            ViewBag.ErrorFechaInicio = null;
            string fecha_inicio = Request["fecha_inicio"];
            ViewBag.ErrorFechaFinal = null;
            string fecha_final = Request["fecha_final"];
            ViewBag.ErrorCentro = null;
            string centro = Request["centro"];
            ViewBag.ErrorHorario = null;
            string horario = Request["horario"];
            ViewBag.ErrorModalidad = null;
            string modalidad = Request["modalidad"];
            ViewBag.ErrorDescripcion = null;
            string descripcion = Request["descripcion"];

            if (String.IsNullOrEmpty(curso_nombre))
                ViewBag.ErrorCursoNombre = "*Este campo es obligatorio";
            else
                ViewData["curso_nombreText"] = curso_nombre;


            if (String.IsNullOrEmpty(num_plaza))
                ViewBag.ErrorNumPlaza = "*Este campo es obligatorio";
            else
                ViewData["num_plazaText"] = num_plaza;

            if (String.IsNullOrEmpty(horas_totales))
                ViewBag.ErrorHorasTotales = "*Este campo es obligatorio";
            else
                ViewData["horas_totalesText"] = horas_totales;

            if (String.IsNullOrEmpty(fecha_inicio))
                ViewBag.ErrorFechaInicio = "*Este campo es obligatorio";
            else
                ViewData["fecha_inicioText"] = fecha_inicio;

            if (String.IsNullOrEmpty(fecha_final))
                ViewBag.ErrorFechaFinal = "*Este campo es obligatorio";

            else if (int.Parse(fecha_final.Replace("-", "")) < int.Parse(fecha_inicio.Replace("-", "")))
                ViewBag.ErrorFechaFinal = "*La fecha final es menor a la inicial";

            else
                ViewData["fecha_finalText"] = fecha_final;

            if (String.IsNullOrEmpty(centro))
                ViewBag.ErrorCentro = "*Este campo es obligatorio";
            else
                ViewData["centroText"] = centro;

            if (String.IsNullOrEmpty(horario))
                ViewBag.ErrorHorario = "*Este campo es obligatorio";
            else
                ViewData["horarioText"] = horario;

            if (String.IsNullOrEmpty(modalidad))
                ViewBag.ErrorModalidad = "*Este campo es obligatorio";
            else
                ViewData["modalidadText"] = modalidad;

            if (String.IsNullOrEmpty(descripcion))
                ViewBag.ErrorDescripcion = "*Este campo es obligatorio";
            else
                ViewData["DescripcionText"] = descripcion;
            /*if (String.IsNullOrEmpty("" + ViewBag.ErrorCursoNombre + ViewBag.ErrorNumPlaza + ViewBag.ErrorHorasTotales + ViewBag.ErrorFechaInicio + ViewBag.ErrorFechaFinal + ViewBag.ErrorCentro + ViewBag.ErrorHorario + ViewBag.ErrorModalidad + ViewBag.ErrorDescripcion))
            {
                DAL_Formador dal_Formador = new DAL_Formador();
                List<Filtro> filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", (Session["User"].ToString().Split('/')[1]), ECondicionText.Igual));
                Formador formador = dal_Formador.Select_Formador(filtros, null).First();
                DAL_Horario dal_Horario = new DAL_Horario();

                Curso curso = new Curso(curso_nombre, descripcion, int.Parse(num_plaza), int.Parse(horas_totales), DateTime.Parse(fecha_inicio), DateTime.Parse(fecha_final), horario, formador, mo );
            }*/
            return View("FormadorPerfilPublicada");
        }
        public ActionResult ListaAlumnos()
        {
            ViewBag.Message = "Your List Alumno page.";

            return View();
        }
    }
}
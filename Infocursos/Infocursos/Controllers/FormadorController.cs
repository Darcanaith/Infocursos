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

        
        public ActionResult FormadorPerfil(int? IdFormador)
        {
            @ViewData["DisplayAddButton"] = "normal";
            @ViewData["DisplayAddDescripcion"] = "none";

            Formador formador=null;
            DAL_Formador dal_Formador = new DAL_Formador();
            List<Filtro> filtros = new List<Filtro>();
            filtros.Add(new Filtro("Id_User", IdFormador.ToString(), ECondicionNum.Ig));

            if (IdFormador == null)
                if (!FormadorIsLoged())
                    return View("../Home/Index");
                else
                    formador = (Formador)Session["User"];
            else
                formador = dal_Formador.Select_Formador(filtros, null).First();
            

            
            return RellenarFormadorPerfil(formador);
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
        public ActionResult Info_Cursos(Formador formador)
        {
            string IsVisible = Request["WhoIsVisible"];
            if(IsVisible.Equals("Info"))
                @ViewData["Mostrar"] = "Curso";
            else
                @ViewData["Mostrar"] = "Info";


            return RellenarFormadorPerfil(formador);
        }

        [HttpPost]
        public ActionResult RellenarFormadorPerfil(Formador formador)
        {

            if (@ViewData["Mostrar"] == null)
                @ViewData["Mostrar"] = "Info";
            SetButtonsInfo_Curso(@ViewData["Mostrar"].Equals("Info"));


            ViewData["IMG_Perfil"] = formador.IMG_Perfil;
            @ViewData["Nombre_Entidad"] = formador.Nombre_Entidad;
            @ViewData["Resumen"] = formador.User_Resumen;

            @ViewData["Descripcion"] = formador.User_Descripcion;

            formador.GetCursos();
            @ViewData["Cursos"] = formador.Cursos;

            @ViewData["Horarios"] = formador.Horarios;
            @ViewData["Modalidades"] = formador.Modalidades;
            @ViewData["Categorias"] = formador.Categorias;

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

            
            return RellenarFormadorPerfil(null);
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

            return RellenarFormadorPerfil(null);
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

            return RellenarFormadorPerfil(null);
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

            return RellenarFormadorPerfil(null);
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

        public void SetButtonLista_cursos(bool cursoIsSelected)
        {
            if (!cursoIsSelected)
            {
                @ViewData["anadir_cursos"] = "none";
                @ViewData["lista_cursos"] = "normal";
                @ViewData["style_button_lista"] = "btn-outline-primary";
                @ViewData["style_button_añadir"] = "btn-primary";
            }
            else
            {
                @ViewData["anadir_cursos"] = "normal";
                @ViewData["lista_cursos"] = "none";
                @ViewData["style_button_lista"] = "btn-primary";
                @ViewData["style_button_añadir"] = "btn-outline-primary";

            }
        }

        [HttpPost]
        public ActionResult MostrarLista_cursos(Formador formador)
        {
            string botonListaCursos = Request["boton_lista"];
             if (botonListaCursos.Equals("boton_lista"))
                @ViewData["Mostrar"] = "lista_cursos";
            else
                @ViewData["Mostrar"] = "añadir_cursos";


            return RellenarListaCurso(formador);
         }

        public ActionResult RellenarListaCurso(Formador formador)
        {
            if (ViewData["Mostrar"] == null)
                @ViewData["Mostrar"] = "añadir_cursos";
            SetButtonLista_cursos(@ViewData["Mostrar"].Equals("añadir_cursos"));

            @ViewData["IMG_Perfil"] = formador.IMG_Perfil;
            formador.GetCursos();
            ViewData["cursos"] = formador.Cursos;
            return View("FormadorPerfilPublicada");
        }


        public ActionResult FormadorPerfilPublicada(int? IdFormador)
        {
            @ViewData["anadir_cursos"] = "normal";
            @ViewData["lista_cursos"] = "none";

            Formador formador = null;
            DAL_Formador dal_Formador = new DAL_Formador();
            List<Filtro> filtros = new List<Filtro>();
            filtros.Add(new Filtro("Id_User", IdFormador.ToString(), ECondicionNum.Ig));

            if (IdFormador == null)
                if (!FormadorIsLoged())
                    return View("../Home/Index");
                else
                    formador = (Formador)Session["User"];
            else
            {
                formador = dal_Formador.Select_Formador(filtros, null).First();
                filtros.Clear();
                DAL_Curso dal_curso = new DAL_Curso();
                filtros.Add(new Filtro("Rid_Formador", ((Usuario)Session["User"]).Id_User.ToString(), ECondicionNum.Ig));
                List<Curso> cursos = dal_curso.Select_Curso(filtros, null);
                DAL_Centro dal_centro = new DAL_Centro();
                DAL_Horario dal_horario = new DAL_Horario();
                DAL_Modalidad dal_modalidad = new DAL_Modalidad();

                List<Filtro> filtros2 = new List<Filtro>();
                List<Centro> centros = new List<Centro>();
                foreach (Curso curso in cursos)
                {
                    if (curso.Centro != null)
                        filtros2.Add(new Filtro("Rid_Centro", curso.Centro.Id_centro.ToString(), ECondicionNum.Ig));
                    else
                        filtros2 = null;
                    centros = dal_centro.Select_Centro(filtros2, null);

                }
                List<Horario> horarios = dal_horario.Select_Horarios(null, null);
                List<Modalidad> modalidades = dal_modalidad.Select_Modalidades(null, null);

                @ViewData["centros"] = centros;
                @ViewData["horarios"] = horarios;
                @ViewData["modalidades"] = modalidades;
            }


            return RellenarListaCurso(formador);
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
            if (centro.Equals("Escoja uno de sus centros si se hace en alguno"))
                centro = null;
            ViewBag.ErrorHorario = null;
            string horario = Request["horario"];
            if (horario.Equals("Selecciona un horario"))
                horario = null;
            ViewBag.ErrorModalidad = null;
            string modalidad = Request["modalidad"];
            if (modalidad.Equals("Seleccione una modalidad"))
                modalidad = null;
            ViewBag.ErrorDescripcion = null;
            string descripcion = Request["curso_descripcion"];

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

            if (String.IsNullOrEmpty(horario))
                ViewBag.ErrorHorario = "*Este campo es obligatorio";

            if (String.IsNullOrEmpty(modalidad))
                ViewBag.ErrorModalidad = "*Este campo es obligatorio";

            if (String.IsNullOrEmpty(descripcion))
                ViewBag.ErrorDescripcion = "*Este campo es obligatorio";
            else
                ViewData["DescripcionText"] = descripcion;
            if (String.IsNullOrEmpty("" + ViewBag.ErrorCursoNombre + ViewBag.ErrorNumPlaza + ViewBag.ErrorHorasTotales + ViewBag.ErrorFechaInicio + ViewBag.ErrorFechaFinal + ViewBag.ErrorHorario + ViewBag.ErrorModalidad + ViewBag.ErrorDescripcion))
            {
                DAL_Formador dal_Formador = new DAL_Formador();
                List<Filtro> filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", ((Usuario)Session["User"]).Email, ECondicionText.Igual));
                Formador formador = dal_Formador.Select_Formador(filtros, null).First();
                DAL_Horario dal_horario = new DAL_Horario();
                filtros.Clear();
                filtros.Add(new Filtro("Tipo_Horario", horario, ECondicionText.Igual));
                Horario horario1 = dal_horario.Select_Horarios(filtros, null).First();
                DAL_Modalidad dal_modalidad = new DAL_Modalidad();
                filtros.Clear();
                filtros.Add(new Filtro("Modalidad", modalidad, ECondicionText.Igual));
                Modalidad modalidad1 = dal_modalidad.Select_Modalidades(filtros, null).First();
                Centro centro1 = null;
                if (centro != null)
                {
                    DAL_Centro dal_centro = new DAL_Centro();
                    filtros.Clear();
                    filtros.Add(new Filtro("Centro_direccion", centro, ECondicionText.Igual));
                    centro1 = dal_centro.Select_Centro(filtros, null).First();
                }

                Curso curso = new Curso(curso_nombre, descripcion, int.Parse(num_plaza), int.Parse(horas_totales), DateTime.Parse(fecha_inicio), DateTime.Parse(fecha_final), horario1, formador, modalidad1, centro1);
                DAL_Curso dal_curso = new DAL_Curso();
                dal_curso.Insert_Cursos(curso);
            }
            return View("FormadorPerfilPublicada");
        }
        public ActionResult ListaAlumnos()
        {
            ViewBag.Message = "Your List Alumno page.";

            return View();
        }
    }
}
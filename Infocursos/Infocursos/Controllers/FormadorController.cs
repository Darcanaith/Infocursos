using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infocursos.DAL;
using Infocursos.Models;

namespace Infocursos.Controllers
{
    public class FormadorController : Controller
    {
        // GET: Formador
        public ActionResult FormadorPerfil()
        {
            return View();
        }
        public ActionResult FormadorEditarPerfil()
        {
            if (ViewData["Formador"]!= null)
            return View();
            else {
                return View("../Home/Index");
            }
        }

        [HttpPost]
        public ActionResult EditFormador()
        {
            string nombre = Request["Nombre"];
            string apellidos = Request["Apellidos"];
            string entidad = Request["Entidad"];
            string resumen = Request["Resumen"];
            Formador formador = (Formador) ViewData["Formador"];
            formador.User_Nombre = nombre;
            formador.User_Apellidos = apellidos;
            formador.Nombre_Entidad = entidad;
            formador.User_Resumen = resumen;
            DAL_Formador dal_formador = new DAL_Formador();
            dal_formador.Update_Formador(formador);
            return View("FormadorEditarPerfil");
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

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
    }
}
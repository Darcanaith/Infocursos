using Infocursos.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Infocursos.Models.Enums;

namespace Infocursos.Models
{
    public class Formador : Usuario
    {
        private bool isAutorizado;
        private string nombre_Entidad;
        private string cod_Validacion;
        private List<Curso> cursos;

        private List<Centro> centros = new List<Centro>();
        private List<string> horarios = new List<string>();
        private List<string> modalidades = new List<string>();
        private List<Categoria> categorias = new List<Categoria>();

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string file { get; set; }
        public bool IsAutorizado { get => isAutorizado; set => isAutorizado = value; }
        public string Nombre_Entidad { get => nombre_Entidad; set => nombre_Entidad = value; }
        public string Cod_Validacion { get => cod_Validacion; set => cod_Validacion = value; }
        public List<Centro> Centros { get => centros; set => centros = value; }
        public List<Curso> Cursos { get => cursos; set => cursos = value; }
        public List<string> Horarios { get => horarios; set => horarios = value; }
        public List<string> Modalidades { get => modalidades; set => modalidades = value; }
        public List<Categoria> Categorias { get => categorias; set => categorias = value; }

        public Formador(string email, string password, string user_Nombre, string user_Apellidos, string nombre_Entidad)
            : base(email, password, user_Nombre, user_Apellidos)
        {
            Nombre_Entidad = nombre_Entidad;
            IsAutorizado = false;
            Cod_Validacion = GenerarCodValidacion();
        }

        public Formador(int id_User, string email, string password, string user_Nombre, string user_Apellidos, string user_Descripcion,
            string user_Resumen, string iMG_Perfil,List<string> telefonos, string nombre_Entidad, string cod_Validacion, bool isAutorizado)
            : base(id_User, email, password, user_Nombre, user_Apellidos, user_Descripcion, user_Resumen, iMG_Perfil, telefonos)
        {
            Nombre_Entidad = nombre_Entidad;
            IsAutorizado = isAutorizado;
            Cod_Validacion = cod_Validacion;
            Cursos = cursos;
        }

        public string GenerarCodValidacion()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Codigo = new char[10];
            var random = new Random();

            for (int i = 0; i < Codigo.Length; i++)
                Codigo[i] = characters[random.Next(characters.Length)];

            return new String(Codigo);
        }
        
        public void GetCursos()
        {
            DAL_Curso dal_Curso = new DAL_Curso();
            List<Filtro> filtros_curso = new List<Filtro>();
            filtros_curso.Add(new Filtro("Rid_Formador", Id_User.ToString(), ECondicionNum.Ig));
            Cursos = dal_Curso.Select_Curso(filtros_curso, null);

            foreach (Curso curso in Cursos)
            {
                Modalidades.Add(curso.Modalidad.Tipo_modalidad);
                Horarios.Add(curso.Horario.Tipo_horario);
                Centros.Add(curso.Centro);
                Categorias.AddRange(curso.Categorias);
            }
            Modalidades = Modalidades.Distinct().ToList();
            Horarios = Horarios.Distinct().ToList();
            Centros = Centros.Distinct().ToList();
            Categorias = Categorias.Distinct().ToList();
        }
    }
}

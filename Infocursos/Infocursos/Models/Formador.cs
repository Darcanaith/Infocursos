using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.Models
{
    public class Formador : User
    {
        private bool isAutorizado;
        private string nombre_Entidad;
        private string cod_Validacion;
        private List<Centro> centros;
        private List<Curso> cursos;
        private List<Horario> horarios;
        private List<Modalidad> modalidades;

        public bool IsAutorizado { get => isAutorizado; set => isAutorizado = value; }
        public string Nombre_Entidad { get => nombre_Entidad; set => nombre_Entidad = value; }
        public string Cod_Validacion { get => cod_Validacion; set => cod_Validacion = value; }
        public List<Centro> Centros { get => centros; set => centros = value; }
        public List<Curso> Cursos { get => cursos; set => cursos = value; }
        public List<Horario> Horarios { get => horarios; set => horarios = value; }
        public List<Modalidad> Modalidades { get => modalidades; set => modalidades = value; }



        public Formador(string email, string password, string user_Nombre, string user_Apellidos, string nombre_Entidad) : base(email, password, user_Nombre, user_Apellidos)
        {
            Nombre_Entidad = nombre_Entidad;
            IsAutorizado = false;
            Cod_Validacion = "RANDOM";
        }
    }
}

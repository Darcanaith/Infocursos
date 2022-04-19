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
        private List<Curso> cursos;

        private List<Centro> centros;
        private List<Horario> horarios;
        private List<Modalidad> modalidades;
        private List<Categoria> categorias;

        public bool IsAutorizado { get => isAutorizado; set => isAutorizado = value; }
        public string Nombre_Entidad { get => nombre_Entidad; set => nombre_Entidad = value; }
        public string Cod_Validacion { get => cod_Validacion; set => cod_Validacion = value; }
        public List<Centro> Centros { get => centros; set => centros = value; }
        public List<Curso> Cursos { get => cursos; set => cursos = value; }
        public List<Horario> Horarios { get => horarios; set => horarios = value; }
        public List<Modalidad> Modalidades { get => modalidades; set => modalidades = value; }
        public List<Categoria> Categorias { get => categorias; set => categorias = value; }

        public Formador(string email, string password, string user_Nombre, string user_Apellidos, string nombre_Entidad) 
            : base(email, password, user_Nombre, user_Apellidos)
        {
            Nombre_Entidad = nombre_Entidad;
            IsAutorizado = false;
            Cod_Validacion = GenerarCodValidacion(); 
        }

        public Formador(int id_User, string email, string password, string user_Nombre, string user_Apellidos, string user_Descripcion, 
            string user_Resumen, string iMG_Perfil, List<string> telefonos, string nombre_Entidad, string cod_Validacion, bool isAutorizado) 
            : base(id_User, email, password, user_Nombre, user_Apellidos, user_Descripcion, user_Resumen, iMG_Perfil, telefonos)
        {
            Nombre_Entidad = nombre_Entidad;
            IsAutorizado = isAutorizado;
            Cod_Validacion = cod_Validacion;
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
    }
}

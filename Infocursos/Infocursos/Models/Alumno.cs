using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.Models
{
    public class Alumno : User
    {
        private DateTime? alumno_FechaNac;
        private string alumno_Direccion;
        private Municipio municipio;
        private List<Categoria> categorias;


        public DateTime? Alumno_FechaNac { get => alumno_FechaNac; set => alumno_FechaNac = value; }
        public string Alumno_Direccion { get => alumno_Direccion; set => alumno_Direccion = value; }
        public Municipio Municipio { get => municipio; set => municipio = value; }
        public List<Categoria> Categorias { get => categorias; set => categorias = value; }

        public Alumno(string email, string password, string user_Nombre, string user_Apellidos) : base(email, password, user_Nombre, user_Apellidos)
        {

        }

        public Alumno(int id_User, string email, string password, string user_Nombre, string user_Apellidos, string user_Descripcion, 
            string user_Resumen, string iMG_Perfil, List<Telefono> telefonos, DateTime? alumno_FechaNac, string alumno_Direccion, 
            Municipio municipio, List<Categoria> categorias) : base(id_User, email, password, user_Nombre, user_Apellidos, user_Descripcion, user_Resumen, iMG_Perfil, telefonos)
        {
            Alumno_FechaNac = alumno_FechaNac;
            Alumno_Direccion = alumno_Direccion;
            Municipio = municipio;
            Categorias = categorias;
        }

    }
}

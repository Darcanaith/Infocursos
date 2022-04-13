using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.Models
{
    public class Alumno : User
    {
        private DateTime alumno_FechaNac;
        private string alumno_Direccion;
        private Municipio municipio;

        public DateTime Alumno_FechaNac { get => alumno_FechaNac; set => alumno_FechaNac = value; }
        public string Alumno_Direccion { get => alumno_Direccion; set => alumno_Direccion = value; }
        public Municipio Municipio { get => municipio; set => municipio = value; }

        public Alumno(string email, string password, string user_Nombre, string user_Apellidos) : base(email, password, user_Nombre, user_Apellidos)
        {

        }
    }
}

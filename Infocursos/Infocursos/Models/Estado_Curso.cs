using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Estado_Curso
    {
        private int id_estado_curso;
        private string estado;

        public int Id_estado_curso { get => id_estado_curso; set => id_estado_curso = value; }
        public string Estado { get => estado; set => estado = value; }

        public Estado_Curso(int id_nivel_idioma, string nivel)
        {
            Id_estado_curso = id_nivel_idioma;
            Estado = nivel;
        }

        public Estado_Curso(string nivel)
        {
            Estado = nivel;
        }
    }
}
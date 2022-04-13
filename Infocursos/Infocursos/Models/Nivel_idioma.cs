using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Nivel_idioma
    {
        private int id_nivel_idioma;
        private string nivel;

        public int Id_nivel_idioma { get => id_nivel_idioma; set => id_nivel_idioma = value; }
        public string Nivel { get => nivel; set => nivel = value; }

        public Nivel_idioma(int id_nivel_idioma, string nivel)
        {
            Id_nivel_idioma = id_nivel_idioma;
            Nivel = nivel;
        }

        public Nivel_idioma(string nivel)
        {
            Nivel = nivel;
        }
    }
}
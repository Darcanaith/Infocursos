using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Idioma
    {
        private int id_idioma;
        private string nombre_idioma;

        public int Id_idioma { get => id_idioma; set => id_idioma = value; }
        public string Nombre_idioma { get => nombre_idioma; set => nombre_idioma = value; }

        public Idioma(int id_idioma, string nombre_idioma)
        {
            Id_idioma = id_idioma;
            Nombre_idioma = nombre_idioma;
        }

        public Idioma(string nombre_idioma)
        {
            Nombre_idioma = nombre_idioma;
        }
    }
}
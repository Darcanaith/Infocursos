using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Municipio
    {
        private int id_municipio;
        private string nombre_municipio;
        private Provincia provincia;

        public int Id_municipio { get => id_municipio; set => id_municipio = value; }
        public string Nombre_municipio { get => nombre_municipio; set => nombre_municipio = value; }
        public Provincia Provincia { get => provincia; set => provincia = value; }

        public Municipio(int id_municipio, string nombre_municipio, Provincia provincia)
        {
            Id_municipio = id_municipio;
            Nombre_municipio = nombre_municipio;
            Provincia = provincia;
        }

        public Municipio(string nombre_municipio, Provincia provincia)
        {
            Nombre_municipio = nombre_municipio;
            Provincia = provincia;
        }
    }
}
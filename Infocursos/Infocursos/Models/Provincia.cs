using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Provincia
    {
        private int id_provincia;
        private string nombre_provincia;

        public int Id_provincia { get => id_provincia; set => id_provincia = value; }
        public string Nombre_provincia { get => nombre_provincia; set => nombre_provincia = value;}

        public Provincia(int id_provincia, string nombre_provincia)
        {
            Id_provincia = id_provincia;
            Nombre_provincia = nombre_provincia;
        }

        public Provincia(string nombre_provincia)
        {
            Nombre_provincia = nombre_provincia;
        }
    }
}
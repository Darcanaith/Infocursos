using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Modalidad
    {
        private int id_modalidad;
        private string tipo_modalidad;


        public int Id_modalidad { get => id_modalidad; set => id_modalidad = value; }
        public string Tipo_modalidad { get => tipo_modalidad; set => tipo_modalidad = value; }

        public Modalidad(int id_modalidad, string tipo_modalidad)
        {
            Id_modalidad = id_modalidad;
            Tipo_modalidad = tipo_modalidad;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Centro
    {
        private int id_centro;
        private string centro_direccion;
        private Municipio municipio;

        public int Id_centro { get => id_centro; set => id_centro = value; }
        public string Centro_direccion { get => centro_direccion; set => centro_direccion = value; }
        public Municipio Municipio { get => municipio; set => municipio = value; }

        public Centro(int id_centro, string centro_direccion, Municipio municipio)
        {
            Id_centro = id_centro;
            Centro_direccion = centro_direccion;
            Municipio = municipio;
        }

        public Centro(string centro_direccion, Municipio municipio)
        {
            Centro_direccion = centro_direccion;
            Municipio = municipio;
        }
    }
}
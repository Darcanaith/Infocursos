using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Categoria
    {
        private int id_categoria;
        private string categoria_nombre;
        private Categoria categoria_mayor;

        public int Id_categoria { get => id_categoria; set => id_categoria = value; }
        public string Categoria_nombre { get => categoria_nombre; set => categoria_nombre = value; }
        public Categoria Categoria_mayor { get => categoria_mayor; set => categoria_mayor = value; }

        public Categoria(int id_categoria, string categoria_nombre, Categoria categoria_mayor)
        {
            Id_categoria = id_categoria;
            Categoria_nombre = categoria_nombre;
            Categoria_mayor = categoria_mayor;
        }

        public Categoria(string categoria_nombre, Categoria categoria_mayor)
        {
            Categoria_nombre = categoria_nombre;
            Categoria_mayor = categoria_mayor;
        }
    }
}
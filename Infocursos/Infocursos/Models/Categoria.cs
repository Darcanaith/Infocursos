using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Categoria
    {
        private int id_categoria;
        private string categoria_Nombre;
        private int rId_Categoria_Mayor;
        private Categoria categoria_Mayor;

        public int Id_categoria { get => id_categoria; set => id_categoria = value; }
        public string Categoria_nombre { get => categoria_Nombre; set => categoria_Nombre = value; }
        public Categoria Categoria_mayor { get => categoria_Mayor; set => categoria_Mayor = value; }
        public int RId_Categoria_Mayor { get => rId_Categoria_Mayor; set => rId_Categoria_Mayor = value; }

        public Categoria(int id_categoria, string categoria_nombre, Categoria categoria_mayor)
        {
            Id_categoria = id_categoria;
            Categoria_nombre = categoria_nombre;
            Categoria_mayor = categoria_mayor;
        }
        public Categoria(int id_categoria, string categoria_nombre, int rid_categoria_mayor)
        {
            Id_categoria = id_categoria;
            Categoria_nombre = categoria_nombre;
            RId_Categoria_Mayor = rid_categoria_mayor;
        }

        public Categoria(string categoria_nombre, Categoria categoria_mayor)
        {
            Categoria_nombre = categoria_nombre;
            Categoria_mayor = categoria_mayor;
        }
    }
}
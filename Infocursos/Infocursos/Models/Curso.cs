using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Curso
    {
        private int id_curso;
        private string curso_nombre;
        private string curso_descripcion;
        private int num_plaza;
        private int horas_totales;
        private DateTime fecha_inicio;
        private DateTime fecha_final;
        private Horario horario;
        private Formador formador;


        public int Id_curso { get => id_curso; set => id_curso = value; }
        public string Curso_nombre { get => curso_nombre; set => curso_nombre = value; }
        public string Curso_descripcion { get => curso_descripcion; set => curso_descripcion = value; }
        public int Num_plaza { get => num_plaza; set => num_plaza = value; }
        public int Horas_totales { get => horas_totales; set => horas_totales = value; }
        public DateTime Fecha_inicio { get => fecha_inicio; set => fecha_inicio = value; }
        public DateTime Fecha_final { get => fecha_final; set => fecha_final = value; }
        public Horario Horario { get => horario; set => horario = value; }
        internal Formador Formador { get => formador; set => formador = value; }

        public Curso(int id_curso, string curso_nombre, string curso_descripcion, int num_plaza, int horas_totales, DateTime fecha_inicio, DateTime fecha_final, Horario horario, Formador formador)
        {
            Id_curso = id_curso;
            Curso_nombre = curso_nombre;
            Curso_descripcion = curso_descripcion;
            Num_plaza = num_plaza;
            Horas_totales = horas_totales;
            Fecha_inicio = fecha_inicio;
            Fecha_final = fecha_final;
            Horario = horario;
            Formador = formador;
        }

        public Curso(string curso_nombre, string curso_descripcion, int num_plaza, int horas_totales, DateTime fecha_inicio, DateTime fecha_final, Horario horario, Formador formador)
        {
            Curso_nombre = curso_nombre;
            Curso_descripcion = curso_descripcion;
            Num_plaza = num_plaza;
            Horas_totales = horas_totales;
            Fecha_inicio = fecha_inicio;
            Fecha_final = fecha_final;
            Horario = horario;
            Formador = formador;
        }
    }
}
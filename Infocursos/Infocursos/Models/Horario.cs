using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Horario
    {
        private int id_horario;
        private string tipo_horario;

        public int Id_horario { get => id_horario; set => id_horario = value; }
        public string Tipo_horario { get => tipo_horario; set => tipo_horario = value; }

        public Horario(int id_horario, string tipo_horario)
        {
            Id_horario = id_horario;
            Tipo_horario = tipo_horario;
        }
    }
}
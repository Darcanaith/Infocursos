using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infocursos.Models
{
    public class Telefono
    {
        private int id_telefono;
        private User user;
        private int numero_telefono;

        public int Id_telefono { get => id_telefono; set => id_telefono = value; }
        public User User { get => user; set => user = value; }
        public int Numero_telefono { get => numero_telefono; set => numero_telefono = value; }

        public Telefono(int id_telefono, User user, int numero_telefono)
        {
            Id_telefono = id_telefono;
            User = user;
            Numero_telefono = numero_telefono;
        }

        public Telefono(User user, int numero_telefono)
        {
            User = user;
            Numero_telefono = numero_telefono;
        }
    }
}
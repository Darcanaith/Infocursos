using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.Models
{
    public class Usuario
    {
        private int id_User;
        private string email;
        private string password;
        private string user_Nombre;
        private string user_Apellidos;
        private string user_Descripcion;
        private string user_Resumen;
        private string iMG_Perfil;
        private List<string> telefonos;


        public int Id_User { get => id_User; set => id_User = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string User_Nombre { get => user_Nombre; set => user_Nombre = value; }
        public string User_Apellidos { get => user_Apellidos; set => user_Apellidos = value; }
        public string User_Descripcion { get => user_Descripcion; set => user_Descripcion = value; }
        public string User_Resumen { get => user_Resumen; set => user_Resumen = value; }
        public string IMG_Perfil { get => iMG_Perfil; set => iMG_Perfil = value; }
        public List<string> Telefonos { get => telefonos; set => telefonos = value; }

        public Usuario(string email, string password, string user_Nombre, string user_Apellidos)
        {
            Email = email;
            Password = password;
            User_Nombre = user_Nombre;
            User_Apellidos = user_Apellidos;
            IMG_Perfil = "huevo.png";
        }

        public Usuario(int id_User, string email, string password, string user_Nombre, string user_Apellidos, string user_Descripcion, string user_Resumen, string iMG_Perfil, List<string> telefonos)
        {
            Id_User = id_User;
            Email = email;
            Password = password;
            User_Nombre = user_Nombre;
            User_Apellidos = user_Apellidos;
            User_Descripcion = user_Descripcion;
            User_Resumen = user_Resumen;
            IMG_Perfil = iMG_Perfil;
            Telefonos = telefonos;
        }



    }
}

using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Usuario</c>
    /// Se encarga de hacer el CRUD de los datos relacionados con la tabla Usuario.
    /// </summary>
    public class DAL_Usuario
    {
        CNX cnx = null;

        public DAL_Usuario()
        {
            cnx = new CNX();
        }
        /// <summary>
        /// Method <c>Select_Usuario</c>
        /// Este metodo genera una lista de alumnos, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros. 
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Una lista de usuarios</returns>
        public List<Usuario> Select_Usuario(List<Filtro> filtros, string orderBy)
        {
            List<Usuario> Usuarios = new List<Usuario>();
            string sentenciaFiltros = "";
            if (filtros != null)
            {
                for (int i = 0; i < filtros.Count; i++)
                {
                    if (i == 0)
                        sentenciaFiltros = " WHERE ";
                    else
                        sentenciaFiltros += " " + filtros[i].Conector + " ";

                    sentenciaFiltros += filtros[i];
                }
            }

            SqlDataReader reader = null;
            try
            {
                string sql = @"SELECT * FROM Usuario" + sentenciaFiltros + " " + orderBy;
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                reader = cdm.ExecuteReader();

                DAL_Telefono dat_telefono = new DAL_Telefono();

                while (reader.Read())
                {

                    string user_Descripcion = null, user_Resumen = null, iMG_Perfil = null;
                    List<string> telefonos_User = new List<string>();

                    if (reader.GetValue(5) != DBNull.Value)
                        user_Descripcion = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        user_Resumen = reader.GetString(6);

                    if (reader.GetValue(7) != DBNull.Value)
                        iMG_Perfil = reader.GetString(7);
                    
                    IDictionary<int, string> telefonos = dat_telefono.Select_Telefono(null, null);
                    foreach (KeyValuePair<int, string> telefono in telefonos)
                        if (telefono.Key == reader.GetInt32(0))
                            telefonos_User.Add(telefono.Value);

                    Usuarios.Add(new Usuario(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), user_Descripcion, user_Resumen, iMG_Perfil, telefonos_User));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return Usuarios;
        }

        /// <summary>
        /// Method <c>Update_Usuario</c>
        /// Este metodo actualiza la tabla Usuario en la base de datos, la fila que se tiene que actualizar viene determinada 
        /// por el id del objeto usuario que se le pasa por parametros. 
        /// </summary>
        /// <param name="usuario">Objecto usuario con los datos actualizados para subir a la base de datos.</param>
        public void Update_Usuario(Usuario usuario)
        {
            try
            {
                string sql_Usuario = @"UPDATE Usuario SET 
                                Email = @Email,
                                Password = @Password, 
                                User_Nombre = @User_Nombre, 
                                User_Apellidos = @User_Apellidos,
                                User_Descripcion = @User_Descripcion,
                                User_Resumen = @User_Resumen,
                                IMG_Perfil = @IMG_Perfil 
                                WHERE Id_User= " + usuario.Id_User + "; ";
                SqlCommand cdm_Usuario = new SqlCommand(sql_Usuario, cnx.Connection);


                SqlParameter pEmail = new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100);
                pEmail.Value = usuario.Email;

                SqlParameter pPassword = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50);
                pPassword.Value = usuario.Password;

                SqlParameter pUser_Nombre = new SqlParameter("@User_Nombre", System.Data.SqlDbType.NVarChar, 50);
                pUser_Nombre.Value = usuario.User_Nombre;

                SqlParameter pUser_Apellidos = new SqlParameter("@User_Apellidos", System.Data.SqlDbType.NVarChar, 100);
                pUser_Apellidos.Value = usuario.User_Apellidos;

                SqlParameter pUser_Descripcion = new SqlParameter("@User_Descripcion", System.Data.SqlDbType.NVarChar, 4000);
                pUser_Descripcion.Value = DBNull.Value;
                if (usuario.User_Descripcion != null)
                    pUser_Descripcion.Value = usuario.User_Descripcion;

                SqlParameter pUser_Resumen = new SqlParameter("@User_Resumen", System.Data.SqlDbType.NVarChar, 250);
                pUser_Resumen.Value = DBNull.Value;
                if (usuario.User_Resumen != null)
                    pUser_Resumen.Value = usuario.User_Resumen;

                SqlParameter pIMG_Perfil = new SqlParameter("@IMG_Perfil", System.Data.SqlDbType.NVarChar, 300);
                pIMG_Perfil.Value = DBNull.Value;
                if (usuario.IMG_Perfil != null)
                    pIMG_Perfil.Value = usuario.IMG_Perfil;


                cdm_Usuario.Parameters.Add(pEmail);
                cdm_Usuario.Parameters.Add(pPassword);
                cdm_Usuario.Parameters.Add(pUser_Nombre);
                cdm_Usuario.Parameters.Add(pUser_Apellidos);
                cdm_Usuario.Parameters.Add(pUser_Descripcion);
                cdm_Usuario.Parameters.Add(pUser_Resumen);
                cdm_Usuario.Parameters.Add(pIMG_Perfil);
                cdm_Usuario.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method <c>Insert_Usuario</c>
        /// Este metodo genera una fila nueva en la tabla Usuario en la base de datos, 
        /// los datos que se rellenan en esa fila son los del objeto alumno que recibe por parametros. 
        /// </summary>
        /// <param name="user">Objecto usuario con los datos nuevos para insertar en la tabla Usuario.</param>
        public void Insert_Usuario(Usuario user)
        {
            try
            {
                string sql_Usuario = @"INSERT INTO Usuario(Email, Password, User_Nombre, User_Apellidos, IMG_Perfil) 
                                        VALUES(@Email, @Password, @User_Nombre, @User_Apellidos, @IMG_Perfil)";
                SqlCommand cdm_Usuario = new SqlCommand(sql_Usuario, cnx.Connection);

                SqlParameter pEmail = new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100);
                pEmail.Value = user.Email;

                SqlParameter pPassword = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50);
                pPassword.Value = user.Password;

                SqlParameter pUser_Nombre = new SqlParameter("@User_Nombre", System.Data.SqlDbType.NVarChar, 50);
                pUser_Nombre.Value = user.User_Nombre;

                SqlParameter pUser_Apellidos = new SqlParameter("@User_Apellidos", System.Data.SqlDbType.NVarChar, 100);
                pUser_Apellidos.Value = user.User_Apellidos;

                SqlParameter pIMG_Perfil = new SqlParameter("@IMG_Perfil", System.Data.SqlDbType.NVarChar, 300);
                pIMG_Perfil.Value = user.IMG_Perfil;

                cdm_Usuario.Parameters.Add(pEmail);
                cdm_Usuario.Parameters.Add(pPassword);
                cdm_Usuario.Parameters.Add(pUser_Nombre);
                cdm_Usuario.Parameters.Add(pUser_Apellidos);
                cdm_Usuario.Parameters.Add(pIMG_Perfil);
                cdm_Usuario.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method <c>Delete_Alumno</c>
        /// Este metodo Elimina una fila en la tabla de Usuario, las filas a eliminar se determina por el id del objeto usuario que se le pasa
        /// por parametro. 
        /// </summary>
        /// <param name="user">Objeto usuario con el id de las filas que hay que eliminar.</param>
        public void Delete_Usuario(Usuario user)
        {
            try
            {
                string sql = @"DELETE FROM User WHERE Id_User='" + user.Id_User + "';";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                cdm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
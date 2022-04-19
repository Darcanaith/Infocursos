using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.DAL
{
    class DAL_Formador
    {
        CNX cnx = null;

        public DAL_Formador()
        {
            cnx = new CNX();
        }

        public List<Formador> Select_Formador(List<Filtro> filtros, string orderBy)
        {
            List<Formador> Formadores = new List<Formador>();
            string sentenciaFiltros = "";
            if (filtros != null)
            {
                for (int i = 0; i < filtros.Count; i++)
                {
                    if (i == 0)
                        sentenciaFiltros = "WHERE ";
                    else
                        sentenciaFiltros += " AND ";

                    sentenciaFiltros += filtros[i];
                }
            }

            try
            {
                string sql = @"SELECT * FROM Usuario INNER JOIN Formador ON Id_User=RId_User " + sentenciaFiltros + " " + orderBy;
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                SqlDataReader reader = cdm.ExecuteReader();

                while (reader.Read())
                {

                    string user_Descripcion = null, user_Resumen = null, iMG_Perfil = null, nombre_Entidad = null;
                    List<string> telefonos_Formador = new List<string>();

                    if (reader.GetValue(5) != DBNull.Value)
                        user_Descripcion = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        user_Resumen = reader.GetString(6);

                    if (reader.GetValue(7) != DBNull.Value)
                        iMG_Perfil = reader.GetString(7);

                    if (reader.GetValue(9) != DBNull.Value)
                        nombre_Entidad = reader.GetString(9);

                    DAL_Telefono dat_telefono = new DAL_Telefono();
                    IDictionary<int, string> telefonos = dat_telefono.Select_Telefono(null, null);
                    foreach (KeyValuePair<int, string> telefono in telefonos)
                        if (telefono.Key == reader.GetInt32(0))
                            telefonos_Formador.Add(telefono.Value);

                    Formador newFormador = new Formador(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), user_Descripcion, user_Resumen, iMG_Perfil, telefonos_Formador, nombre_Entidad);
                }
                reader.Close();

            }
            catch (Exception er)
            {

            }
            return null;
        }
        public void Update_Formador(Formador formador)
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
                                WHERE Id_User= " + formador.Id_User + "; ";
                SqlCommand cdm_Usuario = new SqlCommand(sql_Usuario, cnx.Connection);

                string sql_Formador = @"UPDATE Formador SET 
                                Nombre_Entidad = @Nombre_Entidad,
                                WHERE RId_User= " + formador.Id_User + "; ";
                SqlCommand cdm_Formador = new SqlCommand(sql_Formador, cnx.Connection);


                SqlParameter pEmail = new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100);
                pEmail.Value = formador.Email;

                SqlParameter pPassword = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50);
                pPassword.Value = formador.Password;

                SqlParameter pUser_Nombre = new SqlParameter("@User_Nombre", System.Data.SqlDbType.NVarChar, 100);
                pUser_Nombre.Value = formador.User_Nombre;

                SqlParameter pUser_Apellidos = new SqlParameter("@User_Apellidos", System.Data.SqlDbType.NVarChar, 50);
                pUser_Apellidos.Value = formador.User_Apellidos;

                SqlParameter pUser_Descripcion = new SqlParameter("@User_Descripcion", System.Data.SqlDbType.NVarChar, 4000);
                pUser_Descripcion.Value = DBNull.Value;
                if (formador.User_Descripcion == null)
                    pUser_Descripcion.Value = formador.User_Descripcion;

                SqlParameter pUser_Resumen = new SqlParameter("@User_Resumen", System.Data.SqlDbType.NVarChar, 250);
                pUser_Resumen.Value = DBNull.Value;
                if (formador.User_Resumen == null)
                    pUser_Resumen.Value = formador.User_Resumen;

                SqlParameter pIMG_Perfil = new SqlParameter("@IMG_Perfil", System.Data.SqlDbType.NVarChar, 300);
                pIMG_Perfil.Value = DBNull.Value;
                if (formador.IMG_Perfil == null)
                    pIMG_Perfil.Value = formador.IMG_Perfil;

                SqlParameter pNombre_Entidad = new SqlParameter("@Nombre_Entidad", System.Data.SqlDbType.NVarChar, 100);
                pNombre_Entidad.Value = formador.Nombre_Entidad;


                cdm_Usuario.Parameters.Add(pEmail);
                cdm_Usuario.Parameters.Add(pPassword);
                cdm_Usuario.Parameters.Add(pUser_Nombre);
                cdm_Usuario.Parameters.Add(pUser_Apellidos);
                cdm_Usuario.Parameters.Add(pUser_Descripcion);
                cdm_Usuario.Parameters.Add(pUser_Resumen);
                cdm_Usuario.Parameters.Add(pIMG_Perfil);
                cdm_Usuario.ExecuteNonQuery();

                cdm_Formador.Parameters.Add(pNombre_Entidad);
                cdm_Formador.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                throw;
            }
        }
        public void Insert_Formador(Formador formador)
        {
            try
            {
                string sql_Usuario = @"INSERT INTO Usuario VALUES(@Email, @Password, @User_Nombre, @User_Apellidos)";
                SqlCommand cdm_Usuario = new SqlCommand(sql_Usuario, cnx.Connection);

                string sql_Formador = @"INSERT INTO Formador VALUES(@RId_User, @Nombre_Entidad, @Cod_Validacion, @IsAutorizado)";
                SqlCommand cdm_Formador = new SqlCommand(sql_Usuario, cnx.Connection);


                SqlParameter pEmail = new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100);
                pEmail.Value = formador.Email;

                SqlParameter pPassword = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50);
                pPassword.Value = formador.Password;

                SqlParameter pUser_Nombre = new SqlParameter("@User_Nombre", System.Data.SqlDbType.NVarChar, 50);
                pUser_Nombre.Value = formador.User_Nombre;

                SqlParameter pUser_Apellidos = new SqlParameter("@User_Apellidos", System.Data.SqlDbType.NVarChar, 100);
                pUser_Apellidos.Value = formador.User_Apellidos;

                

                SqlParameter pNombre_Entidad = new SqlParameter("@Nombre_Entidad", System.Data.SqlDbType.NVarChar, 100);
                pNombre_Entidad.Value = formador.Nombre_Entidad;

                SqlParameter pCod_Validacion = new SqlParameter("@Cod_Validacion", System.Data.SqlDbType.NVarChar, 10);
                pCod_Validacion.Value = formador.Cod_Validacion;

                SqlParameter pIsAutorizado = new SqlParameter("@IsAutorizado", System.Data.SqlDbType.Bit);
                pIsAutorizado.Value = formador.User_Apellidos;


                cdm_Usuario.Parameters.Add(pEmail);
                cdm_Usuario.Parameters.Add(pPassword);
                cdm_Usuario.Parameters.Add(pUser_Nombre);
                cdm_Usuario.Parameters.Add(pUser_Apellidos);
                cdm_Usuario.ExecuteNonQuery();


                SqlParameter pRId_User = new SqlParameter("@RId_User", System.Data.SqlDbType.Int);

                DAL_Formador dal_Formador = new DAL_Formador();
                List<Formador> formadors = dal_Formador.Select_Formador(null,null);
                foreach(Formador newFormador in formadors)
                    if(newFormador.Email.Equals(formador.Email))
                        pRId_User.Value = formador.Id_User;

                cdm_Formador.Parameters.Add(pRId_User);
                cdm_Formador.Parameters.Add(pNombre_Entidad);
                cdm_Formador.Parameters.Add(pCod_Validacion);
                cdm_Formador.Parameters.Add(pIsAutorizado);
                cdm_Formador.ExecuteNonQuery();

            }
            catch (Exception er)
            {
                throw;
            }
        }
        public void Delete_Formador(Formador formador)
        {
            try
            {
                string sql = @"DELETE FROM Formador WHERE RId_User='" + formador.Id_User + "';" +
                              "DELETE FROM Usuario WHERE Id_User='" + formador.Id_User + "';";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                cdm.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                //MessageBox.Show("Error eliminando: " + er.Message);
            }
        }

        public void Autorizar_Formador(Formador formador)
        {
            try
            {
                string sql_Formador = @"UPDATE Formador SET 
                                IsAutorizado = @IsAutorizado,
                                WHERE RId_User= " + formador.Id_User + "; ";
                SqlCommand cdm_Formador = new SqlCommand(sql_Formador, cnx.Connection);


                SqlParameter pIsAutorizado = new SqlParameter("@IsAutorizado", System.Data.SqlDbType.Bit);
                pIsAutorizado.Value = formador.IsAutorizado;

                cdm_Formador.Parameters.Add(pIsAutorizado);
                cdm_Formador.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                throw;
            }
        }
    }
}

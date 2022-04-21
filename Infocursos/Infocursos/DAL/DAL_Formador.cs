using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infocursos.Models.Enums;

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

                    Formadores.Add(new Formador(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), user_Descripcion, user_Resumen, iMG_Perfil, telefonos_Formador, nombre_Entidad,reader.GetString(10),reader.GetBoolean(11)));
                }
                reader.Close();

            }
            catch (Exception er)
            {
                throw;
            }
            return Formadores;
        }
        public void Update_Formador(Formador formador)
        {
            try
            {
                DAL_Usuario dal_Usuario = new DAL_Usuario();
                dal_Usuario.Update_Usuario(formador);

                string sql_Formador = @"UPDATE Formador SET 
                                Nombre_Entidad = @Nombre_Entidad 
                                WHERE RId_User= " + formador.Id_User + "; ";
                SqlCommand cdm_Formador = new SqlCommand(sql_Formador, cnx.Connection);

                SqlParameter pNombre_Entidad = new SqlParameter("@Nombre_Entidad", System.Data.SqlDbType.NVarChar, 100);
                pNombre_Entidad.Value = formador.Nombre_Entidad;

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
                DAL_Usuario dal_Usuario = new DAL_Usuario();
                dal_Usuario.Insert_Usuario(formador);

                string sql = @"INSERT INTO Formador(RId_User, Nombre_Entidad, Cod_Validacion, IsAutorizado) VALUES(@RId_User, @Nombre_Entidad, @Cod_Validacion, @IsAutorizado)";

                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);


                SqlParameter pRId_User = new SqlParameter("@RId_User", System.Data.SqlDbType.Int);
                List<Filtro> filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", formador.Email, ECondicionText.Igual));
                List<Usuario> users = dal_Usuario.Select_Usuario(filtros, null);

                pRId_User.Value = users.First().Id_User;

                SqlParameter pNombre_Entidad = new SqlParameter("@Nombre_Entidad", System.Data.SqlDbType.NVarChar, 100);
                pNombre_Entidad.Value = formador.Nombre_Entidad;

                SqlParameter pCod_Validacion = new SqlParameter("@Cod_Validacion", System.Data.SqlDbType.NVarChar, 10);
                pCod_Validacion.Value = formador.Cod_Validacion;

                SqlParameter pIsAutorizado = new SqlParameter("@IsAutorizado", System.Data.SqlDbType.Bit);
                pIsAutorizado.Value = formador.IsAutorizado;

                cdm.Parameters.Add(pRId_User);
                cdm.Parameters.Add(pNombre_Entidad);
                cdm.Parameters.Add(pCod_Validacion);
                cdm.Parameters.Add(pIsAutorizado);
                cdm.ExecuteNonQuery();

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
                string sql = @"DELETE FROM Formador WHERE RId_User='" + formador.Id_User + "';";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                cdm.ExecuteNonQuery();

                DAL_Usuario dal_Usuario = new DAL_Usuario();
                dal_Usuario.Delete_Usuario(formador);
            }
            catch (Exception er)
            {
                throw;
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

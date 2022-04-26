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
    /// <summary>
    /// Class <c>DAL_Formador</c>
    /// Se encarga de hacer el CRUD de los datos relacionados con la tabla Formador.
    /// </summary>
    class DAL_Formador
    {
        CNX cnx = null;

        public DAL_Formador()
        {
            cnx = new CNX();
        }



    public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Formador</c>
        /// Este metodo genera una lista de formadores, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros. 
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Una lista de formadores</returns>
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
                        sentenciaFiltros += " " + filtros[i].Conector + " ";

                    sentenciaFiltros += filtros[i];
                }
            }
            SqlDataReader reader = null;
            try
            {
                string sql = @"SELECT * FROM Usuario INNER JOIN Formador ON Id_User=RId_User " + sentenciaFiltros + " " + orderBy;
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                reader = cdm.ExecuteReader();

                DAL_Curso dal_Curso = new DAL_Curso();
                DAL_Telefono dat_telefono = new DAL_Telefono();

                while (reader.Read())
                {

                    string user_Descripcion = null, user_Resumen = null, iMG_Perfil = null, nombre_Entidad = null;
                    List<string> telefonos_Formador = new List<string>();
                    List<Curso> cursos = new List<Curso>();

                    if (reader.GetValue(5) != DBNull.Value)
                        user_Descripcion = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        user_Resumen = reader.GetString(6);

                    if (reader.GetValue(7) != DBNull.Value)
                        iMG_Perfil = reader.GetString(7);

                    if (reader.GetValue(9) != DBNull.Value)
                        nombre_Entidad = reader.GetString(9);

                    IDictionary<int, string> telefonos = dat_telefono.Select_Telefono(null, null);
                    foreach (KeyValuePair<int, string> telefono in telefonos)
                        if (telefono.Key == reader.GetInt32(0))
                            telefonos_Formador.Add(telefono.Value);

                    Formadores.Add(new Formador(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), user_Descripcion, user_Resumen, iMG_Perfil, telefonos_Formador, nombre_Entidad,reader.GetString(10),reader.GetBoolean(11)));
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
            return Formadores;
        }

        /// <summary>
        /// Method <c>Update_Formador</c>
        /// Este metodo actualiza la tabla de formadores y usuarios en la base de datos, la fila que se tiene que actualizar viene determinada 
        /// por el id del objeto formador que se le pasa por parametros. 
        /// </summary>
        /// <param name="formador">Objecto formador con los datos actualizados para subir a la base de datos.</param>
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method <c>Insert_Formador</c>
        /// Este metodo genera una fila nueva en la tabla formador y usuario en la base de datos, 
        /// los datos que se rellenan en esa fila son los del objeto formador que recibe por parametros. 
        /// </summary>
        /// <param name="formador">Objecto alumno con los datos nuevos para insertar en la tabla Formador.</param>
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method <c>Delete_Formador</c>
        /// Este metodo Elimina una fila en la tabla de formadores y usuarios, las filas a eliminar se determina por el id del objeto formador que se le pasa
        /// por parametro. 
        /// </summary>
        /// <param name="formador">Objeto formador con el id de las filas que hay que eliminar.</param>
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method <c>Autorizar_Formador</c>
        /// Este metodo actualiza el campo IsAutorizado de la tabla Formador de false a true, la fila se determina por el id del objeto formador
        /// que recibe por parametro.;
        /// </summary>
        /// <param name="formador">Objeto formador con el id de formador que hay que autorizar</param>
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}

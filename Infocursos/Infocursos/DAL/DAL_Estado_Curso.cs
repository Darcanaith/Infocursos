using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Estado_Curso</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Estado-Curso.
    /// </summary>
    public class DAL_Estado_Curso
    {
        CNX cnx = null;

        public DAL_Estado_Curso()
        {
            cnx = new CNX();
        }

        /// <summary>
        /// Method <c>Select_Estado_Curso</c>
        /// Este metodo se encarga de hacer un diccionario que relaciona el id de la fila con un objeto estado_curso.
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Un dicionario de id_estado_inscripcion-Estado_Curso.</returns>
        public IDictionary<int, Estado_Curso> Select_Estado_Curso(List<Filtro> filtros, string orderBy)
        {
            IDictionary<int, Estado_Curso> idiomas = new Dictionary<int, Estado_Curso>();
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
                string sql = "SELECT * FROM Estado_Inscripcion" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    idiomas.Add(reader.GetInt32(0), new Estado_Curso(reader.GetInt32(0), reader.GetString(1)));
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
            return idiomas;
        }
    }
}
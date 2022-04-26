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
    /// Se encarga de hacer el select de los datos relacionados con la tabla Idioma.
    /// </summary>
    public class DAL_Idioma
    {
        CNX cnx = null;

        public DAL_Idioma()
        {
            cnx = new CNX();
        }

        /// <summary>
        /// Method <c>Select_Idioma</c>
        /// Este metodo se encarga de hacer un diccionario que relaciona el id de la fila con un objeto idioma.
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Un dicionario de id_idioma-Idioma.</returns>
        public IDictionary<int, Idioma> Select_Idioma(List<Filtro> filtros, string orderBy)
        {
            IDictionary<int, Idioma> idiomas = new Dictionary<int, Idioma>();
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
                string sql = "SELECT * FROM Idioma" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    idiomas.Add(reader.GetInt32(0), new Idioma(reader.GetInt32(0), reader.GetString(1)));
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
using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Nivel_Idioma</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Nivel_Idioma.
    /// </summary>
    public class DAL_Nivel_Idioma
    {
        CNX cnx = null;

        public DAL_Nivel_Idioma()
        {
            cnx = new CNX();
        }

        /// <summary>
        /// Method <c>Select_Nivel_Idioma</c>
        /// Este metodo se encarga de hacer un diccionario que relaciona el id de la fila con un objeto nivel_idioma.
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Un dicionario de id_nivel_idioma-Nivel_Idioma.</returns>
        public IDictionary<int, Nivel_Idioma> Select_Nivel_Idioma(List<Filtro> filtros, string orderBy)
        {
            IDictionary<int, Nivel_Idioma> nivel_Idiomas = new Dictionary<int, Nivel_Idioma>();
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
                string sql = "SELECT * FROM Nivel_Idioma" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    nivel_Idiomas.Add(reader.GetInt32(0), new Nivel_Idioma(reader.GetInt32(0), reader.GetString(1)));
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
            return nivel_Idiomas;
        }
    }
}
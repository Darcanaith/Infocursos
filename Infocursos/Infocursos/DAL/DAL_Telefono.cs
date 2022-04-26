using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Telefono</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Telefono.
    /// </summary>
    public class DAL_Telefono
    {
        CNX cnx = null;

        public DAL_Telefono()
        {
            this.Cnx = new CNX();
        }

        public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Telefono</c>
        /// Este metodo genera una lista de telefonos, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros. 
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>>
        /// <returns>Una lista de telefonos</returns>
        public IDictionary<int, string> Select_Telefono(List<Filtro> filtros, string orderBy)
        {
            IDictionary<int, string> telefonos = new Dictionary<int, string>();
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
                string sql = "SELECT * FROM Telefono" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, Cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    telefonos.Add(reader.GetInt32(0),reader.GetString(1));
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
            return telefonos;
        }
    }
}
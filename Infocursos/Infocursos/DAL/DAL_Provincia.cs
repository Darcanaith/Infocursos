using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infocursos.Models;
using System.Data.SqlClient;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Provincia</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Provincia.
    /// </summary>

    public class DAL_Provincia
    {
        CNX cnx = null;

        public DAL_Provincia()
        {
            Cnx = new CNX();
        }


        public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Provincia</c>
        /// Este metodo genera una lista de provincias, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros. 
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>>
        /// <returns>Una lista de provincias</returns>
        public List<Provincia> Select_Provincia(List<Filtro> filtros, string orderBy)
        {
            List<Provincia> provincias = new List<Provincia>();
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
                string sql = "select * from Provincia " + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, Cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Provincia provincia = new Provincia(reader.GetInt32(0), reader.GetString(1));
                    provincias.Add(provincia);
                }
                reader.Close();
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
            return provincias;
        }
    }
}
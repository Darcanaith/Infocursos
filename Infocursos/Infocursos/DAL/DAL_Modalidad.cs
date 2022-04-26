using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infocursos.Models;
using System.Data.SqlClient;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Modalidad</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Modalidad.
    /// </summary>
    public class DAL_Modalidad
    {
        CNX cnx = null;

        public DAL_Modalidad()
        {
            this.cnx = new CNX();
        }

    public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Modalidades</c>
        /// Este metodo genera una lista de modalidades, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros. 
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Una lista de modalidades</returns>
        public List<Modalidad> Select_Modalidades(List<Filtro> filtros, string orderBy)
        {
            List<Modalidad> modalidades = new List<Modalidad>();
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
                string sql = "select * from Modalidad " + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Modalidad modalidad = new Modalidad(reader.GetInt32(0), reader.GetString(1));
                    modalidades.Add(modalidad);
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
            return modalidades;

        }
    }
}
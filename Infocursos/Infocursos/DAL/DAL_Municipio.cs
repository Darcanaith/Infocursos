using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infocursos.Models;
using System.Data.SqlClient;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Municipio</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Municipio.
    /// </summary>
    public class DAL_Municipio
    {
        CNX cnx = null;

        public DAL_Municipio()
        {
            this.cnx = new CNX();
        }

    public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Municipio</c>
        /// Este metodo genera una lista de municipio, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros. 
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Una lista de municipios</returns>
        public List<Municipio> Select_Municipio(List<Filtro> filtros, string orderBy)
        {
            List<Municipio> municipios = new List<Municipio>();
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
                string sql = "select * from Municipio " + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                Provincia provincia = null;
                reader = cmd.ExecuteReader();

                DAL_Provincia dal_provincia = new DAL_Provincia();
                while (reader.Read())
                {
                    
                    List<Provincia> provincias = dal_provincia.Select_Provincia(null, null);
                    foreach (Provincia provincia_de_lista in provincias)
                    {
                        if (reader.GetInt32(2)== provincia_de_lista.Id_provincia)
                        {
                            provincia = provincia_de_lista;
                        }
                    }
                    Municipio municipio = new Municipio(reader.GetInt32(0), reader.GetString(1), provincia);
                    municipios.Add(municipio);
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
            return municipios;

        }
    }
}
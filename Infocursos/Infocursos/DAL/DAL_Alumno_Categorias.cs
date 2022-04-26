using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Alumno_Categorias</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Alumnos-Categoria.
    /// </summary>
    public class DAL_Alumno_Categorias
    {
        CNX cnx = null;

        public DAL_Alumno_Categorias()
        {
            cnx = new CNX();
        }


    public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Alumno_Categorias</c>
        /// Este metodo se encarga de hacer un diccionario que relaciona una fila de la tabla de alumno-categoria 
        /// con el id foranea de la categoria que este guardada en esa fila.
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Un dicionario de alumno_categoria-categoria.</returns>
        public IDictionary<int[], int> Select_Alumno_Categorias(List<Filtro> filtros, string orderBy)
        {
            IDictionary<int[], int> alumno_Categoria = new Dictionary<int[], int>();

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
                string sql = "select * from Alumno_Categorias" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int[] clave = new int[2] { reader.GetInt32(0), reader.GetInt32(1) };
                    alumno_Categoria.Add(clave, reader.GetInt32(1));
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
            return alumno_Categoria;
        }
    }
}
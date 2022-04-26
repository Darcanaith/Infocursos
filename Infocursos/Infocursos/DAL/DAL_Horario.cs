using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infocursos.DAL
{
    /// <summary>
    /// Class <c>DAL_Horario</c>
    /// Se encarga de hacer el select de los datos relacionados con la tabla Horario.
    /// </summary>
    public class DAL_Horario
    {
        CNX cnx = null;

        public DAL_Horario()
        {
            this.cnx = new CNX();
        }


    public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Alumno</c>
        /// Este metodo genera una lista de horarios, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros. 
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Una lista de horarios</returns>
        public List<Horario> Select_Horarios(List<Filtro> filtros, string orderBy)
        {
            List<Horario> horarios = new List<Horario>();
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
                string sql = "select * from Horario " + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Horario horario = new Horario(reader.GetInt32(0), reader.GetString(1));
                    horarios.Add(horario);
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
            return horarios;
        }
    }
}
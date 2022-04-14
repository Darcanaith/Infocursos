using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    public class DAL_Alumno_Idioma
    {
        CNX cnx = null;

        public DAL_Alumno_Idioma()
        {
            cnx = new CNX();
        }

        public IDictionary<int[], int> Select_Alumno_Idioma(List<string> filtros, string orderBy)
        {
            IDictionary<int[], int> alumno_Idioma = new Dictionary<int[], int>();

            string sentenciaFiltros = "";
            if (filtros != null)
            {
                for (int i = 0; i < filtros.Count; i++)
                {
                    if (i == 0)
                        sentenciaFiltros = "WHERE ";
                    else
                        sentenciaFiltros += " AND ";

                    sentenciaFiltros += filtros[i];
                }
            }

            try
            {
                string sql = "SELECT * FROM Alumno_Idioma" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int[] clave = new int[2] { reader.GetInt32(0), reader.GetInt32(1) };
                    alumno_Idioma.Add(clave, reader.GetInt32(2));
                }

                reader.Close();

            }
            catch (Exception er)
            {
                throw;
            }
            return alumno_Idioma;
        }
    }
}
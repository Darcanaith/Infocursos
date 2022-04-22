using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    public class DAL_Nivel_Idioma
    {
        CNX cnx = null;

        public DAL_Nivel_Idioma()
        {
            cnx = new CNX();
        }

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
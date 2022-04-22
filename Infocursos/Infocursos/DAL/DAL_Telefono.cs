using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    public class DAL_Telefono
    {
        CNX cnx = null;

        public DAL_Telefono()
        {
            this.cnx = new CNX();
        }

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
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
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
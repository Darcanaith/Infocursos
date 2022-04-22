using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infocursos.Models;
using System.Data.SqlClient;

namespace Infocursos.DAL
{
    public class DAL_Provincia
    {
        CNX cnx = null;

        public DAL_Provincia()
        {
            this.cnx = new CNX();
        }

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
                        sentenciaFiltros += " AND ";

                    sentenciaFiltros += filtros[i];
                }
            }

            SqlDataReader reader = null;
            try
            {
                string sql = "select * from Provincia" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Provincia provincia = new Provincia(reader.GetInt32(0), reader.GetString(1));
                    provincias.Add(provincia);
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
            return provincias;
        }
    }
}
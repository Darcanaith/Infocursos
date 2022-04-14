using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infocursos.Models;
using System.Data.SqlClient;

namespace Infocursos.DAL
{
    public class DAL_Municipio
    {
        CNX cnx = null;

        public DAL_Municipio()
        {
            this.cnx = new CNX();
        }

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
                        sentenciaFiltros += " AND ";

                    sentenciaFiltros += filtros[i];
                }
            }
            try
            {
                string sql = "select * from Municipio" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                Provincia provincia = null;
                SqlDataReader  reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DAL_Provincia dal_provincia = new DAL_Provincia();
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
            return municipios;

        }
    }
}
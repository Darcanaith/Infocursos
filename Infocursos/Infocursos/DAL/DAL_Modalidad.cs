using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infocursos.Models;
using System.Data.SqlClient;

namespace Infocursos.DAL
{
    public class DAL_Modalidad
    {
        CNX cnx = null;

        public DAL_Modalidad()
        {
            this.cnx = new CNX();
        }

    public CNX Cnx { get => cnx; set => cnx = value; }
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
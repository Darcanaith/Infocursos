using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.DAL
{
    public class DAL_Centro
    {
        CNX cnx = null;

        public DAL_Centro()
        {
            this.cnx = new CNX();
        }

        public List<Centro> Select_Centro(List<Filtro> filtros, string orderBy)
        {
            List<Centro> centros = new List<Centro>();
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
                string sql = "Select * from Centro" + sentenciaFiltros + " " + orderBy + ";";
                Municipio municipio = null;
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DAL_Municipio dal_municipio = new DAL_Municipio();
                    List<Municipio> municipios = dal_municipio.Select_Municipio(null, null);
                    foreach (Municipio municipio_de_lista in municipios)
                    {
                        if (reader.GetInt32(2) == municipio_de_lista.Id_municipio)
                        {
                            municipio = municipio_de_lista;
                        }
                    }
                    Centro centro = new Centro(reader.GetInt32(0), reader.GetString(1), municipio);
                    centros.Add(centro);

                }

            }
            catch (Exception)
            {

                throw;
            }
            return centros;
        }
    }
}
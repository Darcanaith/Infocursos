using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    public class DAL_Curso_Categorias
    {
        CNX cnx = null;

        public DAL_Curso_Categorias()
        {
            cnx = new CNX();
        }

        public IDictionary<int[], int> Select_Curso_Categorias(List<string> filtros, string orderBy)
        {
            IDictionary<int[], int> curso_Categoria = new Dictionary<int[], int>();

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
                string sql = "select * from Curso_Categorias" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int[] clave = new int[2] { reader.GetInt32(0), reader.GetInt32(1) };
                    curso_Categoria.Add(clave, reader.GetInt32(1));
                }

                reader.Close();

            }
            catch (Exception er)
            {
                throw;
            }
            return curso_Categoria;
        }
    }
}
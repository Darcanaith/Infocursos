using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infocursos.DAL
{
    public class DAL_Horario
    {
        CNX cnx = null;

        public DAL_Horario()
        {
            this.cnx = new CNX();
        }

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
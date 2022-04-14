using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.DAL
{
    class DAL_Formador
    {
        CNX cnx = null;

        public DAL_Formador()
        {
            cnx = new CNX();
        }

        public List<Formador> Select_Formador(List<Filtro> filtros, string orderBy)
        {
            List<Formador> Formadores = new List<Formador>();
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
                string sql = @"SELECT * FROM Usuario INNER JOIN Formador ON Id_User=RId_User " + sentenciaFiltros + " " + orderBy;
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                SqlDataReader reader = cdm.ExecuteReader();

                while (reader.Read())
                {

                    string user_Descripcion = null, user_Resumen = null, iMG_Perfil = null, nombre_Entidad = null;
                    List<string> telefonos_Formador = new List<string>();
                    List<Curso> cursos_Formador = new List<Curso>();

                    if (reader.GetValue(5) != DBNull.Value)
                        user_Descripcion = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        user_Resumen = reader.GetString(6);

                    if (reader.GetValue(7) != DBNull.Value)
                        iMG_Perfil = reader.GetString(7);

                    if (reader.GetValue(9) != DBNull.Value)
                        nombre_Entidad = reader.GetString(9);

                    DAL_Telefono dat_telefono = new DAL_Telefono();
                    IDictionary<int, string> telefonos = dat_telefono.Select_Telefono(null, null);
                    foreach (KeyValuePair<int, string> telefono in telefonos)
                        if (telefono.Key == reader.GetInt32(0))
                            telefonos_Formador.Add(telefono.Value);

                    Formador newFormador = new Formador(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), user_Descripcion, user_Resumen, iMG_Perfil, telefonos_Formador, nombre_Entidad);
                }
                reader.Close();

            }
            catch (Exception er)
            {

            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infocursos.Models;
using System.Data.SqlClient;

namespace Infocursos.DAL
{
    public class DAL_Curso
    {
        CNX cnx = null;

        public DAL_Curso(CNX cnx)
        {
            this.cnx = new CNX();
        }

        public List<Curso> Select_Curso(List<Filtro> filtros, string orderBy)
        {
            List<Curso> cursos = new List<Curso>();
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
                string sql = "select  * from Curso " + sentenciaFiltros + " " + orderBy +  ";";
                SqlCommand sqlCommand = new SqlCommand(sql, cnx.Connection);
                Horario horario = null;
                Formador formador = null;
                Modalidad modalidad = null;
                Centro centro = null;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    string sql_categoria_horario = "select Horario.* from Curso inner join Horario on Rid_Horario = Id_Horario where Id_curso = " + reader.GetInt32(0) + ";";
                    SqlCommand sqlCommand_categoria_horario = new SqlCommand(sql, cnx.Connection);
                    SqlDataReader reader_categoria_horario = sqlCommand_categoria_horario.ExecuteReader();
                    while (reader_categoria_horario.Read())
                    {
                        horario.Id_horario = reader_categoria_horario.GetInt32(0);
                    }

                    string sql_categoria_formador = "select Formador.* from Curso inner join Formador on Rid_Formador = RId_Userinner join Usuario on RId_User = id_user where Id_curso" + reader.GetInt32(0) + ";";
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
            return cursos;
        }
    }
}
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

        public DAL_Curso()
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
                    DAL_Horario dal_horario = new DAL_Horario();
                    List<Horario> horarios = dal_horario.Select_Horarios(null, null);
                    foreach (Horario horario_de_lista in horarios)
                    {
                        if (reader.GetInt32(7) == horario_de_lista.Id_horario)
                        {
                            horario = horario_de_lista;
                        }
                    }

                    DAL_Modalidad dal_modalidad = new DAL_Modalidad();
                    List<Modalidad> modalidades = dal_modalidad.Select_Modalidades(null, null);
                    foreach (Modalidad modalidad_de_lista in modalidades)
                    {
                        if(reader.GetInt32(9) == modalidad_de_lista.Id_modalidad)
                        {
                            modalidad = modalidad_de_lista;
                        }

                    }
                    DAL_Centro dal_centro = new DAL_Centro();
                    List<Centro> centros = dal_centro.Select_Centro(null,null);
                    foreach (Centro centro_de_lista in centros)
                    {
                        if (reader.GetInt32(10) == centro_de_lista.Id_centro)
                        {
                            centro = centro_de_lista;
                        }
                    }
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
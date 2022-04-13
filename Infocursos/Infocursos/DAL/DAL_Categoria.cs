using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos.DAL
{
    public class DAL_Categoria
    {
        CNX cnx = null;

        public DAL_Categoria()
        {
            cnx = new CNX();
        }

        public List<Alumno> Select_Alumno(List<Filtro> filtros, string orderBy)
        {
            List<Alumno> alumnos = new List<Alumno>();
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
                string sql = @"SELECT * FROM User INNER JOIN Alumno ON Id_User=RId_User " + sentenciaFiltros + " " + orderBy;

                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                SqlDataReader dr = cdm.ExecuteReader();


                while (dr.Read())
                { 

                    if (dr.GetValue(2) == DBNull.Value)
                        min_salary = null;
                    else
                        min_salary = dr.GetDecimal(2);

                    if (dr.GetValue(3) == DBNull.Value)
                        max_salary = null;
                    else
                        max_salary = dr.GetDecimal(3);

                    Alumno newAlumno = new Alumno();
                }
                dr.Close();

            }
            catch (Exception er)
            {

            }
            return null;
        }
    }
}
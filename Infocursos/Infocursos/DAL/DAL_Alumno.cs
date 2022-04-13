using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.DAL
{
    class DAL_Alumno
    {
        CNX cnx = null;

        public DAL_Alumno()
        {
            cnx = new CNX();
        }

        public void Select_Alumno(List<Filtro> filtros, string orderBy)
        {
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
                string sql = @"SELECT * FROM jobs " + sentenciaFiltros + " " + orderBy;

                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                SqlDataReader dr = cdm.ExecuteReader();
                Decimal? min_salary, max_salary;


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


                }
                dr.Close();
            }
            catch (Exception er)
            {
                
            }
        }

        public void Update_Alumno(Alumno alumno)
        {
            try
            {
                string sql = @"UPDATE jobs
                            SET job_title = @job_title, min_salary = @min_salary, max_salary = @max_salary
                            WHERE job_id= " + alumno.job_id + "; ";

                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);


                SqlParameter pjob_title = new SqlParameter("@job_title", System.Data.SqlDbType.VarChar, 35);
                pjob_title.Value = job.job_title;

                SqlParameter pmin_salary = new SqlParameter("@min_salary", System.Data.SqlDbType.Decimal);
                pmin_salary.Precision = 8;
                pmin_salary.Scale = 2;
                if (job.min_salary.HasValue)
                    pmin_salary.Value = job.min_salary;
                else
                    pmin_salary.Value = DBNull.Value;

                SqlParameter pmax_salary = new SqlParameter("@max_salary", System.Data.SqlDbType.Decimal);
                pmax_salary.Precision = 8;
                pmax_salary.Scale = 2;
                if (job.max_salary.HasValue)
                    pmax_salary.Value = job.max_salary;
                else
                    pmax_salary.Value = DBNull.Value;


                cdm.Parameters.Add(pjob_title);
                cdm.Parameters.Add(pmin_salary);
                cdm.Parameters.Add(pmax_salary);


                cdm.ExecuteNonQuery();
                MessageBox.Show("Job actualizado correctamente");
            }
            catch (Exception er)
            {
                MessageBox.Show("Error actualizando: " + er.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
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

        public void Select_Alumno()
        {
            List<Job> jobs = new List<Job>();
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
                string sql = @"SELECT * FROM jobs " + sentenciaFiltros + " " + orederBy;

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

                    Job job = new Job(dr.GetInt32(0), dr.GetString(1), min_salary, max_salary);
                    jobs.Add(job);
                }
                dr.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error Mostrando: " + er.Message);
            }

            return jobs;
        }
    }
}

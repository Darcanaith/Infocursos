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
                    List<Categoria> categorias = new List<Categoria>();

                    try
                    {
                        string sql_Categorias = @"SELECT Categoria.* FROM Alumno_Categorias INNER JOIN Categoria ON Id_Categoria=RId_Categoria WHERE RId_Alumno= "+dr.GetInt32(0);
                        SqlCommand cdm_Categorias = new SqlCommand(sql_Categorias, cnx.Connection);
                        SqlDataReader dr_Categorias = cdm_Categorias.ExecuteReader();

                        while (dr_Categorias.Read())
                        {
                            Categoria categoria_mayor;
                            try
                            {
                                string sql_Categoria_Mayor = @"SELECT Categoria.* FROM Categoria INNER JOIN Categoria ON Id_Categoria=RId_Categoria_Mayor WHERE Id_Categoria= " + dr_Categorias.GetInt32(0);
                                SqlCommand cdm_Categoria_Mayor = new SqlCommand(sql_Categoria_Mayor, cnx.Connection);
                                SqlDataReader dr_Categoria_Mayor = cdm_Categoria_Mayor.ExecuteReader();

                                while (dr_Categoria_Mayor.Read())
                                {
                                    categoria_mayor = new Categoria();
                                }
                                dr_Categoria_Mayor.Close();
                            }
                            catch
                            {
                                throw;
                            }

                            Categoria categoria = new Categoria(dr_Categorias.GetInt32(0),dr_Categorias.GetString(1), null);
                        }
                        dr_Categorias.Close();
                    }
                    catch
                    {
                        throw;
                    }

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

        /*public void Update_Alumno(Alumno alumno)
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
        }*/



        public void Insert_Alumno(Alumno alumno)
        {
            try
            {
                string sql = @"INSERT INTO User VALUES(@Email @Password, @User_Nombre, @User_Apellidos)";

                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);


                SqlParameter pEmail = new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100);
                pEmail.Value = alumno.Email;

                SqlParameter pPassword = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50);
                pPassword.Value = alumno.Password;

                SqlParameter pUser_Nombre = new SqlParameter("@User_Nombre", System.Data.SqlDbType.NVarChar, 100);
                pUser_Nombre.Value = alumno.User_Nombre;

                SqlParameter pUser_Apellidos = new SqlParameter("@User_Apellidos", System.Data.SqlDbType.NVarChar, 50);
                pUser_Apellidos.Value = alumno.User_Apellidos;


                cdm.Parameters.Add(pEmail);
                cdm.Parameters.Add(pPassword);
                cdm.Parameters.Add(pUser_Nombre);
                cdm.Parameters.Add(pUser_Apellidos);
                cdm.ExecuteNonQuery();

                //MessageBox.Show("Alumno creado correctamente");
            }
            catch (Exception er)
            {
                //MessageBox.Show("Error creando: " + er.Message);
            }
        }
        public void DeleteJob(Alumno alumno)
        {
            try
            {
                string sql = @"DELETE FROM jobs WHERE job_id='" + job.job_id + "';";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                cdm.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error eliminando: " + er.Message);
            }
        }
    }
}
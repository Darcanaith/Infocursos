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
                SqlDataReader reader = cdm.ExecuteReader();

                DAL_Categoria dal_Categoria = new DAL_Categoria();
                IDictionary<int, Categoria> categorias = dal_Categoria.Select_Categoria(null,null);

                while (reader.Read())
                {
                    List<Categoria> categorias_Alumno = new List<Categoria>();
                    string user_Descripcion = null, user_Resumen = null, iMG_Perfil = null, alumno_Direccion = null;
                    DateTime? alumno_FechaNac = null;
                    Municipio municipio=null;

                    DAL_Alumno_Categorias dal_Alumno_Categorias = new DAL_Alumno_Categorias();
                    IDictionary<int[], int> idAlumno_Idcategorias = dal_Alumno_Categorias.Select_Alumno_Categorias(null,null);
                    foreach (KeyValuePair<int[],int> kp in idAlumno_Idcategorias)
                        if (reader.GetInt32(0) == kp.Key[0])
                            categorias_Alumno.Add(categorias[kp.Value]);

                    if (reader.GetValue(5) != DBNull.Value)
                        user_Descripcion = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        user_Resumen = reader.GetString(6);

                    if (reader.GetValue(7) != DBNull.Value)
                        iMG_Perfil = reader.GetString(7);

                    if (reader.GetValue(9) != DBNull.Value)
                        alumno_FechaNac = reader.GetDateTime(9);

                    if (reader.GetValue(10) != DBNull.Value)
                        alumno_Direccion = reader.GetString(10);

                    if (reader.GetValue(11) != DBNull.Value)
                    {
                        DAL_Municipio dal_municipio = new DAL_Municipio();
                        List<Municipio> municipios = dal_municipio.Select_Municipio(null, null);
                        foreach (Municipio municipio_de_lista in municipios)
                        {
                            if (reader.GetInt32(11) == municipio_de_lista.Id_municipio)
                            {
                                municipio = municipio_de_lista;
                            }
                        }
                    }

                    Alumno newAlumno = new Alumno(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),user_Descripcion,user_Resumen,iMG_Perfil,null,alumno_FechaNac,alumno_Direccion,municipio,categorias_Alumno);
                }
                reader.Close();

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
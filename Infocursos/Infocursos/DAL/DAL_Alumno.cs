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
                string sql = @"SELECT * FROM Usuario INNER JOIN Alumno ON Id_User=RId_User " + sentenciaFiltros + " " + orderBy;
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
                    List<Object[]> idiomas_Nivel = new List<object[]>();
                    List<string> telefonos_Alumno = new List<string>();

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
                    DAL_Idioma dal_Idioma = new DAL_Idioma();
                    IDictionary<int, Idioma> idiomas = dal_Idioma.Select_Idioma(null, null);
                    DAL_Nivel_Idioma dal_Nivel_Idioma = new DAL_Nivel_Idioma();
                    IDictionary<int, Nivel_Idioma> niveles_Idioma = dal_Nivel_Idioma.Select_Nivel_Idioma(null, null);

                    DAL_Alumno_Idioma dal_Alumno_Idioma = new DAL_Alumno_Idioma();
                    IDictionary<int[], int> alumno_Idioma = dal_Alumno_Idioma.Select_Alumno_Idioma(null, null);
                    foreach (KeyValuePair<int[], int> alumno_Idioma_Nivel in alumno_Idioma)
                        if (reader.GetInt32(0) == alumno_Idioma_Nivel.Key[0])
                            idiomas_Nivel.Add(new object[] { idiomas[alumno_Idioma_Nivel.Key[1]], niveles_Idioma[alumno_Idioma_Nivel.Value] });

                    DAL_Telefono dat_telefono = new DAL_Telefono();
                    IDictionary<int, string> telefonos = dat_telefono.Select_Telefono(null, null);
                    foreach (KeyValuePair<int, string> telefono in telefonos)
                        if (telefono.Key == reader.GetInt32(0))
                            telefonos_Alumno.Add(telefono.Value);

                        Alumno newAlumno = new Alumno(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),user_Descripcion,user_Resumen,iMG_Perfil, telefonos_Alumno, alumno_FechaNac,alumno_Direccion,municipio,categorias_Alumno, idiomas_Nivel);
                }
                reader.Close();

            }
            catch (Exception er)
            {

            }
            return null;
        }

        public void Update_Alumno(Alumno alumno)
        {
            try
            {
                string sql_Usuario = @"UPDATE Usuario SET 
                                Email = @Email,
                                Password = @Password, 
                                User_Nombre = @User_Nombre, 
                                User_Apellidos = @User_Apellidos,
                                User_Descripcion = @User_Descripcion,
                                User_Resumen = @User_Resumen,
                                IMG_Perfil = @IMG_Perfil
                                WHERE Id_User= " + alumno.Id_User + "; ";
                SqlCommand cdm_Usuario = new SqlCommand(sql_Usuario, cnx.Connection);

                string sql_Alumno = @"UPDATE Alumno SET 
                                Alumno_FechaNac = @Alumno_FechaNac,
                                Alumno_Direccion = @Alumno_Direccion, 
                                RId_Municipio = @RId_Municipio
                                WHERE RId_User= " + alumno.Id_User + "; ";
                SqlCommand cdm_Alumno = new SqlCommand(sql_Alumno, cnx.Connection);


                SqlParameter pEmail = new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100);
                pEmail.Value = alumno.Email;

                SqlParameter pPassword = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50);
                pPassword.Value = alumno.Password;

                SqlParameter pUser_Nombre = new SqlParameter("@User_Nombre", System.Data.SqlDbType.NVarChar, 100);
                pUser_Nombre.Value = alumno.User_Nombre;

                SqlParameter pUser_Apellidos = new SqlParameter("@User_Apellidos", System.Data.SqlDbType.NVarChar, 50);
                pUser_Apellidos.Value = alumno.User_Apellidos;

                SqlParameter pUser_Descripcion = new SqlParameter("@User_Descripcion", System.Data.SqlDbType.NVarChar, 4000);
                pUser_Descripcion.Value = DBNull.Value;
                if (alumno.User_Descripcion == null)
                    pUser_Descripcion.Value = alumno.User_Descripcion;

                SqlParameter pUser_Resumen = new SqlParameter("@User_Resumen", System.Data.SqlDbType.NVarChar, 250);
                pUser_Resumen.Value = DBNull.Value;
                if (alumno.User_Resumen == null)
                    pUser_Resumen.Value = alumno.User_Resumen;

                SqlParameter pIMG_Perfil = new SqlParameter("@IMG_Perfil", System.Data.SqlDbType.NVarChar, 300);
                pIMG_Perfil.Value = DBNull.Value;
                if (alumno.IMG_Perfil == null)
                    pIMG_Perfil.Value = alumno.IMG_Perfil;

                SqlParameter pAlumno_FechaNac = new SqlParameter("@Alumno_FechaNac", System.Data.SqlDbType.Date);
                pAlumno_FechaNac.Value = DBNull.Value;
                if (alumno.Alumno_FechaNac != null)
                    pAlumno_FechaNac.Value = alumno.Alumno_FechaNac;

                SqlParameter pAlumno_Direccion = new SqlParameter("@Alumno_Direccion", System.Data.SqlDbType.NVarChar, 150);
                pAlumno_Direccion.Value = DBNull.Value;
                if (alumno.Alumno_Direccion == null)
                    pAlumno_Direccion.Value = alumno.Alumno_Direccion;

                SqlParameter pRId_Municipio = new SqlParameter("@RId_Municipio", System.Data.SqlDbType.Int);
                pRId_Municipio.Value = DBNull.Value;
                if (alumno.Municipio == null)
                    pRId_Municipio.Value = alumno.Municipio.Id_municipio;

                cdm_Usuario.Parameters.Add(pEmail);
                cdm_Usuario.Parameters.Add(pPassword);
                cdm_Usuario.Parameters.Add(pUser_Nombre);
                cdm_Usuario.Parameters.Add(pUser_Apellidos);
                cdm_Usuario.Parameters.Add(pUser_Descripcion);
                cdm_Usuario.Parameters.Add(pUser_Resumen);
                cdm_Usuario.Parameters.Add(pIMG_Perfil);
                cdm_Usuario.ExecuteNonQuery();

                cdm_Alumno.Parameters.Add(pAlumno_FechaNac);
                cdm_Alumno.Parameters.Add(pAlumno_Direccion);
                cdm_Alumno.Parameters.Add(pRId_Municipio);
                cdm_Alumno.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                throw;
            }
        }
        public void Insert_Alumno(Alumno alumno)
        {
            try
            {
                string sql = @"INSERT INTO Usuario VALUES(@Email, @Password, @User_Nombre, @User_Apellidos)";

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

            }
            catch (Exception er)
            {
                throw;
            }
        }
        public void Delete_Alumno(Alumno alumno)
        {
            try
            {
                string sql = @"DELETE FROM Alumno WHERE RId_User='" + alumno.Id_User + "';" +
                              "DELETE FROM Usuario WHERE Id_User='" + alumno.Id_User + "';";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                cdm.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                //MessageBox.Show("Error eliminando: " + er.Message);
            }
        }
    }
}
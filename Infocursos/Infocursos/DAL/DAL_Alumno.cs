using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infocursos.Models.Enums;

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
                        sentenciaFiltros = " WHERE ";
                    else
                        sentenciaFiltros += " AND ";

                    sentenciaFiltros += filtros[i];
                }
            }

            SqlDataReader reader = null;
            try
            {
                string sql = @"SELECT * FROM Usuario INNER JOIN Alumno ON Id_User=RId_User" + sentenciaFiltros + " " + orderBy;
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                reader = cdm.ExecuteReader();

                DAL_Categoria dal_Categoria = new DAL_Categoria();
                IDictionary<int, Categoria> categorias = dal_Categoria.Select_Categoria(null,null);
                DAL_Alumno_Categorias dal_Alumno_Categorias = new DAL_Alumno_Categorias();
                DAL_Idioma dal_Idioma = new DAL_Idioma();
                DAL_Alumno_Idioma dal_Alumno_Idioma = new DAL_Alumno_Idioma();
                DAL_Nivel_Idioma dal_Nivel_Idioma = new DAL_Nivel_Idioma();
                DAL_Telefono dat_telefono = new DAL_Telefono();


                while (reader.Read())
                {
                    List<Categoria> categorias_Alumno = new List<Categoria>();
                    string user_Descripcion = null, user_Resumen = null, iMG_Perfil = null, alumno_Direccion = null;
                    DateTime? alumno_FechaNac = null;
                    Municipio municipio=null;
                    List<Object[]> idiomas_Nivel = new List<object[]>();
                    List<string> telefonos_Alumno = new List<string>();

                    
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
                            if (reader.GetInt32(11) == municipio_de_lista.Id_municipio)
                                municipio = municipio_de_lista;
                    }
                    
                    IDictionary<int, Idioma> idiomas = dal_Idioma.Select_Idioma(null, null);   
                    IDictionary<int, Nivel_Idioma> niveles_Idioma = dal_Nivel_Idioma.Select_Nivel_Idioma(null, null);

                    
                    IDictionary<int[], int> alumno_Idioma = dal_Alumno_Idioma.Select_Alumno_Idioma(null, null);
                    foreach (KeyValuePair<int[], int> alumno_Idioma_Nivel in alumno_Idioma)
                        if (reader.GetInt32(0) == alumno_Idioma_Nivel.Key[0])
                            idiomas_Nivel.Add(new object[] { idiomas[alumno_Idioma_Nivel.Key[1]], niveles_Idioma[alumno_Idioma_Nivel.Value] });

                    
                    IDictionary<int, string> telefonos = dat_telefono.Select_Telefono(null, null);
                    foreach (KeyValuePair<int, string> telefono in telefonos)
                        if (telefono.Key == reader.GetInt32(0))
                            telefonos_Alumno.Add(telefono.Value);

                    alumnos.Add(new Alumno(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),user_Descripcion,user_Resumen,iMG_Perfil, telefonos_Alumno, alumno_FechaNac,alumno_Direccion,municipio,categorias_Alumno, idiomas_Nivel));
                }
                
            }
            catch (Exception er)
            {
                throw;
            }
            finally
            {
                if(reader != null)
                    reader.Close();
            }
            return alumnos;
        }

        public void Update_Alumno(Alumno alumno)
        {
            try
            {
                DAL_Usuario dal_Usuario = new DAL_Usuario();
                dal_Usuario.Update_Usuario(alumno);

                string sql_Alumno = @"UPDATE Alumno SET 
                                Alumno_FechaNac = @Alumno_FechaNac,
                                Alumno_Direccion = @Alumno_Direccion, 
                                RId_Municipio = @RId_Municipio
                                WHERE RId_User= " + alumno.Id_User + "; ";
                SqlCommand cdm_Alumno = new SqlCommand(sql_Alumno, cnx.Connection);

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
                DAL_Usuario dal_Usuario = new DAL_Usuario();
                dal_Usuario.Insert_Usuario(alumno);

                string sql = @"INSERT INTO Alumno(RId_User) 
                                VALUES(@RId_User)";

                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);


                SqlParameter pRId_User = new SqlParameter("@RId_User", System.Data.SqlDbType.Int);

                List<Filtro> filtros = new List<Filtro>();
                filtros.Add(new Filtro("Email", alumno.Email, ECondicionText.Igual));
                List<Usuario> users = dal_Usuario.Select_Usuario(filtros, null);
                pRId_User.Value = users.First().Id_User;

                cdm.Parameters.Add(pRId_User);
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
                string sql = @"DELETE FROM Alumno WHERE RId_User='" + alumno.Id_User + "';";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                cdm.ExecuteNonQuery();

                DAL_Usuario dal_Usuario = new DAL_Usuario();
                dal_Usuario.Delete_Usuario(alumno);
            }
            catch (Exception er)
            {
                //MessageBox.Show("Error eliminando: " + er.Message);
            }
        }
    }
}
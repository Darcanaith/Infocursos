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
    /// <summary>
    /// Class <c>DAL_Alumno</c>
    /// Se encarga de hacer el CRUD de los datos relacionados con la tabla Alumno.
    /// </summary>
    class DAL_Alumno
    {
        CNX cnx = null;

        public DAL_Alumno()
        {
            cnx = new CNX();
        }

        /// <summary>
        /// Method <c>Select_Alumno</c>
        /// Este metodo genera una lista de alumnos, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros.
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Una lista de alumnos</returns>
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
                        sentenciaFiltros += " " + filtros[i].Conector + " ";

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
                DAL_Municipio dal_municipio = new DAL_Municipio();

                DAL_Curso dal_curso = new DAL_Curso();
                DAL_Alumno_Curso dal_Alumno_Curso = new DAL_Alumno_Curso();
                DAL_Estado_Curso dal_Estado_Curso = new DAL_Estado_Curso();


                while (reader.Read())
                {
                    List<Categoria> categorias_Alumno = new List<Categoria>();
                    string user_Descripcion = null, user_Resumen = null, iMG_Perfil = null, alumno_Direccion = null;
                    DateTime? alumno_FechaNac = null;
                    Municipio municipio=null;
                    List<Object[]> idiomas_Nivel = new List<object[]>();
                    List<Object[]> cursos_Estado = new List<object[]>();
                    List<string> telefonos_Alumno = new List<string>();
                    List<Curso> cursos = new List<Curso>();
                    List<Filtro> filtros_Alumno = new List<Filtro>();
                    List<Filtro> filtros_Curso = new List<Filtro>();


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

                    
                    IDictionary<int, Estado_Curso> estado_curso = dal_Estado_Curso.Select_Estado_Curso(null, null);

                    filtros_Alumno.Add(new Filtro("RId_Alumno", reader.GetInt32(0).ToString(), ECondicionNum.Ig));
                    IDictionary<int[], int> alumno_Curso = dal_Alumno_Curso.Select_Alumno_Curso(filtros_Alumno, null);

                    foreach (KeyValuePair<int[], int> alumno_Curso_Estado in alumno_Curso)
                    {
                        filtros_Curso.Clear();
                        filtros_Curso.Add(new Filtro("Id_Curso", alumno_Curso_Estado.Key[1].ToString(),ECondicionNum.Ig));
                        cursos_Estado.Add(new object[] { dal_curso.Select_Curso(filtros_Curso, null).First(), estado_curso[alumno_Curso_Estado.Value]  });
                    }

                    IDictionary<int, string> telefonos = dat_telefono.Select_Telefono(null, null);
                    foreach (KeyValuePair<int, string> telefono in telefonos)
                        if (telefono.Key == reader.GetInt32(0))
                            telefonos_Alumno.Add(telefono.Value);

                    alumnos.Add(new Alumno(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),user_Descripcion,user_Resumen,iMG_Perfil, telefonos_Alumno, alumno_FechaNac,alumno_Direccion,municipio,categorias_Alumno, idiomas_Nivel, cursos_Estado));
                }
                
            }
            catch (Exception)
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

        /// <summary>
        /// Method <c>Update_Alumno</c>
        /// Este metodo actualiza la tabla de alumnos y usuarios en la base de datos, la fila que se tiene que actualizar viene determinada 
        /// por el id del objeto alumno que se le pasa por parametros.
        /// </summary>
        /// <param name="alumno">Objecto alumno con los datos actualizados para subir a la base de datos.</param>
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method <c>Insert_Alumno</c>
        /// Este metodo genera una fila nueva en la tabla alumno y usuario en la base de datos, 
        /// los datos que se rellenan en esa fila son los del objeto alumno que recibe por parametros.
        /// </summary>
        /// <param name="alumno">Objecto alumno con los datos nuevos para insertar en la tabla alumnos.</param>
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method <c>Delete_Alumno</c>
        /// Este metodo Elimina una fila en la tabla de alumnos y usuarios, las filas a eliminar se determina por el id del objeto alumno que se le pasa
        /// por parametro.
        /// </summary>
        /// <param name="alumno">Objeto alumno con el id de la fila que hay que eliminar.</param>
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
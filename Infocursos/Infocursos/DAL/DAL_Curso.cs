﻿using System;
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
            SqlDataReader reader = null;
            try
            {
                string sql = "select  * from Curso " + sentenciaFiltros + " " + orderBy +  ";";
                SqlCommand sqlCommand = new SqlCommand(sql, cnx.Connection);
                reader = sqlCommand.ExecuteReader();

                DAL_Categoria dal_Categoria = new DAL_Categoria();
                IDictionary<int, Categoria> categorias = dal_Categoria.Select_Categoria(null, null);

                DAL_Curso_Categorias dal_Curso_Categorias = new DAL_Curso_Categorias();
                DAL_Horario dal_horario = new DAL_Horario();
                DAL_Formador dal_formador = new DAL_Formador();
                DAL_Modalidad dal_modalidad = new DAL_Modalidad();
                DAL_Centro dal_centro = new DAL_Centro();


                while (reader.Read())
                {
                    Horario horario = null;
                    Formador formador = null;
                    Modalidad modalidad = null;
                    Centro centro = null;
                    List<Categoria> categorias_Curso = new List<Categoria>();
                
                    IDictionary<int[], int> idAlumno_Idcategorias = dal_Curso_Categorias.Select_Curso_Categorias(null, null);
                    foreach (KeyValuePair<int[], int> kp in idAlumno_Idcategorias)
                        if (reader.GetInt32(0) == kp.Key[0])
                            categorias_Curso.Add(categorias[kp.Value]);
          
                    List<Horario> horarios = dal_horario.Select_Horarios(null, null);
                    foreach (Horario horario_de_lista in horarios)
                        if (reader.GetInt32(7) == horario_de_lista.Id_horario)
                            horario = horario_de_lista;
  
                    List<Formador> formadores = dal_formador.Select_Formador(null, null);
                    foreach (Formador formador_de_lista in formadores)
                        if (reader.GetInt32(8) == formador_de_lista.Id_User)
                            formador = formador_de_lista;
                
                    List<Modalidad> modalidades = dal_modalidad.Select_Modalidades(null, null);
                    foreach (Modalidad modalidad_de_lista in modalidades)
                        if(reader.GetInt32(9) == modalidad_de_lista.Id_modalidad)
                            modalidad = modalidad_de_lista;


                    if (reader.GetValue(10)!= DBNull.Value)
                    {
                        List<Centro> centros = dal_centro.Select_Centro(null, null);
                        foreach (Centro centro_de_lista in centros)
                            if (reader.GetInt32(9) == centro_de_lista.Id_centro)
                                centro = centro_de_lista;
                    }
                    Curso curso = new Curso(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetDateTime(5), reader.GetDateTime(6), horario, formador, modalidad, centro, categorias_Curso);
                    cursos.Add(curso);
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
            return cursos;
        }

        public void Insert_Cursos(Curso curso)
        {
            try
            {
                string sql = "insert into Curso values(@curso_nombre, @curso_descripcion, @num_plaza, @horas_totales, @fecha_inicio, @fecha_final, @rid_horario, @rid_formador, @rid_modalidad, @rid_centro)";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);

                SqlParameter pcurso_nombre = new SqlParameter("@curso_nombre", System.Data.SqlDbType.NVarChar, 50);
                pcurso_nombre.Value = curso.Curso_nombre;

                SqlParameter pcurso_decripcion = new SqlParameter("@curso_descripcion", System.Data.SqlDbType.NVarChar, 4000);
                pcurso_decripcion.Value = curso.Curso_descripcion;

                SqlParameter pnum_plaza = new SqlParameter("@num_plaza", System.Data.SqlDbType.Int);
                pnum_plaza.Value = curso.Num_plaza;

                SqlParameter phoras_totales = new SqlParameter("@horas_totales", System.Data.SqlDbType.Int);
                phoras_totales.Value = curso.Horas_totales;

                SqlParameter pfecha_inicio = new SqlParameter("@fecha_inicio", System.Data.SqlDbType.Date);
                pfecha_inicio.Value = curso.Fecha_inicio;

                SqlParameter pfecha_final = new SqlParameter("@fecha_final", System.Data.SqlDbType.Date);
                pfecha_final.Value = curso.Fecha_final;

                SqlParameter prid_horario = new SqlParameter("@rid_horario", System.Data.SqlDbType.Int);
                prid_horario.Value = curso.Horario.Id_horario;

                SqlParameter prid_formador = new SqlParameter("@rid_formador", System.Data.SqlDbType.Int);
                prid_formador.Value = curso.Formador.Id_User;

                SqlParameter prid_modalidad = new SqlParameter("@rid_modalidad", System.Data.SqlDbType.Int);
                prid_modalidad.Value = curso.Modalidad.Id_modalidad;

                SqlParameter prid_centro = new SqlParameter("@rid_centro", System.Data.SqlDbType.Int);
                if (curso.Centro == null)
                    prid_centro.Value = DBNull.Value;
                else
                    prid_centro.Value = curso.Centro.Id_centro;

                cmd.Parameters.Add(pcurso_nombre);
                cmd.Parameters.Add(pcurso_decripcion);
                cmd.Parameters.Add(pnum_plaza);
                cmd.Parameters.Add(phoras_totales);
                cmd.Parameters.Add(pfecha_inicio);
                cmd.Parameters.Add(pfecha_final);
                cmd.Parameters.Add(prid_horario);
                cmd.Parameters.Add(prid_formador);
                cmd.Parameters.Add(prid_modalidad);
                cmd.Parameters.Add(prid_centro);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void Update_Curso(Curso curso)
        {
            try
            {
                string sql = @"update Curso set 
                                curso_nombre = @curso_nombre,
                                curso_descripcion = @curso_descripcion,
                                num_plaza = @num_plaza,
                                horas_totales = @horas_totales,
                                fecha_inicio = @fecha_inicio,
                                fecha_final = @fecha_final,
                                RId_Horario = @rid_horario,
                                RId_Formador = @rid_formador,
                                RId_Modalidad = @rid_modalidad,
                                RId_Centro = @rid_centro,
                                where Id_Curso = " + curso.Id_curso + "";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);

                SqlParameter pcurso_nombre = new SqlParameter("@curso_nombre", System.Data.SqlDbType.NVarChar, 50);
                pcurso_nombre.Value = curso.Curso_nombre;

                SqlParameter pcurso_decripcion = new SqlParameter("@curso_descripcion", System.Data.SqlDbType.NVarChar, 4000);
                pcurso_decripcion.Value = curso.Curso_descripcion;

                SqlParameter pnum_plaza = new SqlParameter("@num_plaza", System.Data.SqlDbType.Int);
                pnum_plaza.Value = curso.Num_plaza;

                SqlParameter phoras_totales = new SqlParameter("@horas_totales", System.Data.SqlDbType.Int);
                phoras_totales.Value = curso.Horas_totales;

                SqlParameter pfecha_inicio = new SqlParameter("@fecha_inicio", System.Data.SqlDbType.Date);
                pfecha_inicio.Value = curso.Fecha_inicio;

                SqlParameter pfecha_final = new SqlParameter("@fecha_final", System.Data.SqlDbType.Date);
                pfecha_final.Value = curso.Fecha_final;

                SqlParameter prid_horario = new SqlParameter("@rid_horario", System.Data.SqlDbType.Int);
                prid_horario.Value = curso.Horario.Id_horario;

                SqlParameter prid_formador = new SqlParameter("@rid_formador", System.Data.SqlDbType.Int);
                prid_formador.Value = curso.Formador.Id_User;

                SqlParameter prid_modalidad = new SqlParameter("@rid_modalidad", System.Data.SqlDbType.Int);
                prid_modalidad.Value = curso.Modalidad.Id_modalidad;

                SqlParameter prid_centro = new SqlParameter("@rid_centro", System.Data.SqlDbType.Int);
                if (curso.Centro == null)
                    prid_centro.Value = DBNull.Value;
                else
                    prid_centro.Value = curso.Centro.Id_centro;

                cmd.Parameters.Add(pcurso_nombre);
                cmd.Parameters.Add(pcurso_decripcion);
                cmd.Parameters.Add(pnum_plaza);
                cmd.Parameters.Add(phoras_totales);
                cmd.Parameters.Add(pfecha_inicio);
                cmd.Parameters.Add(pfecha_final);
                cmd.Parameters.Add(prid_horario);
                cmd.Parameters.Add(prid_formador);
                cmd.Parameters.Add(prid_modalidad);
                cmd.Parameters.Add(prid_centro);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Delete_Curso(Curso curso)
        {
            try
            {
                string sql = "delete from Curso where id_curso = '" + curso.Id_curso + "';";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
using Infocursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.DAL
{
    public class DAL_Centro
    {
        /// <summary>
        /// Class <c>DAL_Centro</c>
        /// Se encarga de hacer el CRUD de los datos relacionados con la tabla Centro.
        /// </summary>
        CNX cnx = null;

        public DAL_Centro()
        {
            this.cnx = new CNX();
        }

    public CNX Cnx { get => cnx; set => cnx = value; }

        /// <summary>
        /// Method <c>Select_Centro</c>
        /// Este metodo genera una lista de centros, la cual es distinta dependiendo de los filtros 
        /// y forma de ordenar que se pasen por parametros.
        /// </summary>
        /// <param name="filtros">Es para filtrar los datos recibidos de la base de datos.</param>
        /// <param name="orderBy">Es para ordenar los datos recibidos de la base de datos.</param>
        /// <returns>Una lista de centros.</returns>
        public List<Centro> Select_Centro(List<Filtro> filtros, string orderBy)
        {
            List<Centro> centros = new List<Centro>();
            string sentenciaFiltros = "";
            if (filtros != null)
            {
                for (int i = 0; i < filtros.Count; i++)
                {
                    if (i == 0)
                        sentenciaFiltros = "WHERE ";
                    else
                        sentenciaFiltros += " " + filtros[i].Conector + " ";

                    sentenciaFiltros += filtros[i];
                }
            }
            

            SqlDataReader reader = null;
            try
            {
                string sql = "Select * from Centro " + sentenciaFiltros + " " + orderBy + ";";
                Municipio municipio = null;
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                reader = cmd.ExecuteReader();
                DAL_Municipio dal_municipio = new DAL_Municipio();

                while (reader.Read())
                {
                    List<Municipio> municipios = dal_municipio.Select_Municipio(null, null);
                    foreach (Municipio municipio_de_lista in municipios)
                    {
                        if (reader.GetInt32(2) == municipio_de_lista.Id_municipio)
                        {
                            municipio = municipio_de_lista;
                        }
                    }
                    Centro centro = new Centro(reader.GetInt32(0), reader.GetString(1), municipio);
                    centros.Add(centro);
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return centros;
        }

        /// <summary>
        /// Method <c>Insert_Centro</c>
        /// Este metodo genera una fila nueva en la tabla Centro en la base de datos, 
        /// los datos que se rellenan en esa fila son los del objeto centro que recibe por parametros.
        /// </summary>
        /// <param name="centro">Objecto cnetro con los datos nuevos para subir a la base de datos.</param>
        public void Insert_Centro(Centro centro)
        {
            try
            {
                string sql = "insert into Centro values(@centro_direccion, @Rid_municipio)";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);

                SqlParameter pCentro_direccion = new SqlParameter("@centro_direccion", System.Data.SqlDbType.NVarChar, 150);
                pCentro_direccion.Value = centro.Centro_direccion;

                SqlParameter pRid_municipio = new SqlParameter("@Rid_municipio", System.Data.SqlDbType.Int);
                pRid_municipio.Value = centro.Municipio.Id_municipio;

                cdm.Parameters.Add(pCentro_direccion);
                cdm.Parameters.Add(pRid_municipio);
                cdm.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        /// <summary>
        /// Method <c>Delete_Centro</c>
        /// Este metodo Elimina una fila en la tabla de centros, las filas a eliminar se determina por el id del objeto centro que se le pasa
        /// por parametro.
        /// </summary>
        /// <param name="centro">Objeto centro con el id de la fila que hay que eliminar.</param>
        public void Delete_Centro(Centro centro)
        {
            try
            {
                string sql = "delete from Centro where id_centro = '" + centro.Id_centro + "';";
                SqlCommand cdm = new SqlCommand(sql, cnx.Connection);
                cdm.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
﻿using Infocursos.Models;
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

        public IDictionary<int, Categoria> Select_Categoria(List<string> filtros, string orderBy)
        {
            List<Categoria> categorias_sinCatMay = new List<Categoria>();

            IDictionary<int, Categoria> categorias = new Dictionary<int, Categoria>();
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
                string sql = "select * from Categoria" + sentenciaFiltros + " " + orderBy + ";";
                SqlCommand cmd = new SqlCommand(sql, cnx.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    if (reader.GetValue(2) == DBNull.Value)
                        categorias.Add(reader.GetInt32(0), new Categoria(reader.GetInt32(0), reader.GetString(1), null));
                    else
                        categorias_sinCatMay.Add(new Categoria(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));

                reader.Close();

                do
                {
                    List<Categoria> temp = new List<Categoria>();
                    foreach (Categoria categoria in categorias_sinCatMay)
                    {
                        foreach (KeyValuePair<int, Categoria> categoria_Mayor in categorias)
                            if (categoria.RId_Categoria_Mayor == categoria_Mayor.Value.Id_categoria)
                            {
                                categorias.Add(categoria.Id_categoria, new Categoria(categoria.Id_categoria, categoria.Categoria_nombre, categoria_Mayor.Value));
                                temp.Add(categoria);
                                break;
                            }
                    }
                    foreach (Categoria tempCat in temp)
                        categorias_sinCatMay.Remove(tempCat);

                } while (categorias_sinCatMay.Count > 0);


            }
            catch (Exception er)
            {
                throw;
            }
            return categorias;
        }
    }
}
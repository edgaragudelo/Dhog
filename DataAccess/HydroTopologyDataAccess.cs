using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class HydroTopologyDataAccess
    {
        private static string table = "topologiaHidraulica";

        public static List<HydroTopology> GetSystemTopology(string system)
        {
            List<HydroTopology> hydroTopology = new List<HydroTopology>();

            string query = string.Format("SELECT Sistema, Elemento, Tipo, TipoElemento, Id " +
                                         "FROM {0} " +
                                         "WHERE Sistema = '{1}' " +
                                         "ORDER BY Elemento", table, system);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                hydroTopology.Add(new HydroTopology(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), Convert.ToInt32(reader.GetValue(4))));
            
            DataBaseManager.DbConnection.Close();
            return hydroTopology;
        }

        public static int UpdateObject(HydroTopology dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT Sistema " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Sistema, Elemento, Tipo, TipoElemento) " +
                                      "VALUES('{1}', '{2}', '{3}', '{4}')",
                                      table, dataObject.System, dataObject.Element,
                                      dataObject.Type, dataObject.ElementType);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Sistema = '{1}', " +
                                        "Elemento = '{2}', " +
                                        "Tipo = '{3}', " +
                                        "TipoElemento = '{4}' " +
                                        "WHERE Id = {5}",
                                        table, dataObject.System, dataObject.Element,
                                        dataObject.Type, dataObject.ElementType, dataObject.Id);
            }
            DataBaseManager.DbConnection.Close();
            DataBaseManager.ExecuteQuery(query);

            if (isNew)
            {
                query = string.Format("SELECT Max(Id) FROM {0}", table);
                reader = DataBaseManager.ReadData(query);
                reader.Read();
                int id = Convert.ToInt32(reader.GetValue(0));
                DataBaseManager.DbConnection.Close();
                return id;
            }
            else
                return -1;
        }

        public static void DeleteElement(HydroTopology dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Sistema = '{1}' " +
                                         "AND Elemento = '{2}' " +
                                         "AND Tipo = '{3}'",
                                         table, dataObject.System, dataObject.Element, dataObject.Type);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class ExcludingPlantsDataAccess
    {
        private static string table = "recursosExcluyentes";

        public static List<ExcludingPlants> GetObjects()
        {
            List<ExcludingPlants> excludingPlants = new List<ExcludingPlants>();

            string query = string.Format("SELECT Recurso1, Recurso2 " +
                                         "FROM {0} " +
                                         "ORDER BY Recurso1", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                excludingPlants.Add(new ExcludingPlants(reader.GetString(0), reader.GetString(1)));
            
            DataBaseManager.DbConnection.Close();
            return excludingPlants;
        }

        public static void UpdateObject(ExcludingPlants dataObject)
        {
            string query = string.Format("INSERT INTO {0}(Recurso1, Recurso2) " +
                                         "VALUES('{1}', '{2}')", table, dataObject.Plant1, dataObject.Plant2);
            DataBaseManager.ExecuteQuery(query);
        }

        public static void DeleteObject(ExcludingPlants dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Recurso1 = '{1}' AND Recurso2 = '{2}' ", table, dataObject.Plant1, dataObject.Plant2);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

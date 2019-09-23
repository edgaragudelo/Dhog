using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class RutasDhogParametersDataAccess
    {
        private static string table = "RutasDhog";

        public static List<RutasDhogParameter> GetObjects()
        {
            List<RutasDhogParameter> RutasDhogParameters = new List<RutasDhogParameter>();

            string query = string.Format("SELECT Id, RutaModelo, RutaEjecutable,RutaBD, RutaSalida, RutaSolver " +
                                         "FROM {0} " +
                                         "WHERE id = 1 " +
                                         "ORDER BY id", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                RutasDhogParameters.Add(new RutasDhogParameter(Convert.ToInt32(reader.GetValue(0)), reader.GetString(1), reader.GetString(2), reader.GetString(3),reader.GetString(4),reader.GetString(5)));
            
            DataBaseManager.DbConnection.Close();
            return RutasDhogParameters;
        }

        public static void UpdateObject(RutasDhogParameter dataObject)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "RutaModelo =  '{1}' " + ", RutaBD = '{3}'" + ", RutaEjecutable = '{4}'" + ", RutaSalida = '{5}'" + ", RutaSolver = '{6}'" +
                                         " WHERE ID = {2}", table, dataObject.RutaModelo, dataObject.Id,dataObject.RutaBD,dataObject.RutaEjecutable,dataObject.RutaSalida,dataObject.RutaSolver);
            DataBaseManager.ExecuteQuery(query);

            
        }
    }
}

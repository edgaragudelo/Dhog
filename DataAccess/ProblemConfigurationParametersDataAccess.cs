using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class ProblemConfigurationParametersDataAccess
    {
        private static string table = "configuracionProblema";

        public static List<ProblemConfigurationParameter> GetObjects(int priority)
        {
            List<ProblemConfigurationParameter> problemConfigurationParameters = new List<ProblemConfigurationParameter>();

            string query = string.Format("SELECT nombre, valor, nombreUI " +
                                         "FROM {0} " +
                                         "WHERE PrioridadUI = {1} " +
                                         "ORDER BY nombre", table, priority);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                problemConfigurationParameters.Add(new ProblemConfigurationParameter(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), reader.GetString(2)));
            
            DataBaseManager.DbConnection.Close();
            return problemConfigurationParameters;
        }

        public static void UpdateObject(ProblemConfigurationParameter dataObject)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "valor = {1} " +
                                         "WHERE nombre = '{2}'", table, dataObject.Value, dataObject.Name);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

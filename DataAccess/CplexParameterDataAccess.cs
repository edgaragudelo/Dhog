using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class CplexParametersDataAccess
    {
        private static string table = "parametrosCplex";

        public static List<CplexParameter> GetObjects()
        {
            List<CplexParameter> cplexParameters = new List<CplexParameter>();

            string query = string.Format("SELECT nombre, valor, descripcion " +
                                         "FROM {0} " +
                                         "ORDER BY nombre", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                cplexParameters.Add(new CplexParameter(reader.GetString(0), Convert.ToDouble(reader.GetValue(1)), reader.GetString(2)));
            
            DataBaseManager.DbConnection.Close();
            return cplexParameters;
        }

        public static void UpdateObject(CplexParameter dataObject)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "valor = {1} " +
                                         "WHERE nombre = '{2}'", table, dataObject.Value, dataObject.Name);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

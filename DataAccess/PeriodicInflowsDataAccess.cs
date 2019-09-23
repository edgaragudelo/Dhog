using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicInflowsDataAccess
    {
        public static string table = "aportesHidricos";

        public static List<PeriodicInflow> GetPeriodicInflows()
        {
            List<PeriodicInflow> periodicInflows = new List<PeriodicInflow>();

            string query = string.Format("SELECT Nombre, Periodo, Valor, Escenario " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicInflows.Add(new PeriodicInflow(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();

            return periodicInflows;
        }

        public static void UpdatePeriodicInflow(PeriodicInflow periodicInflow)
        {
            string query =  string.Format("UPDATE {0} SET " +
                                          "Valor = @Value " +
                                          "WHERE nombre = @Name AND " +
                                          "periodo = @Period AND " +
                                          "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Value", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Value"].Value = periodicInflow.Value;
                command.Parameters["@Name"].Value = periodicInflow.Name;
                command.Parameters["@Period"].Value = periodicInflow.Period;
                command.Parameters["@Case"].Value = periodicInflow.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicInflow(PeriodicInflow periodicInflow)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = '{1}' " +
                                         "AND Periodo = {2} " +
                                         "AND Escenario = {3}", 
                                         table, periodicInflow.Name, periodicInflow.Period, periodicInflow.Case);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


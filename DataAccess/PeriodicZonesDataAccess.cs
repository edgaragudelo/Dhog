using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicZonesDataAccess
    {
        public static string table = "zonaPeriodo";

        public static List<PeriodicZone> GetPeriodicZones()
        {
            List<PeriodicZone> periodicZones = new List<PeriodicZone>();

            string query = String.Format("SELECT nombre, Periodo, tipo, valor " +
                           "FROM {0} " +
                           "ORDER BY nombre, Periodo ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicZones.Add(new PeriodicZone(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), reader.GetString(2), Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return periodicZones;
        }

        public static void UpdatePeriodicZone(PeriodicZone periodicZone)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "tipo = @Type, " +
                                         "valor = @Value " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Type", OleDbType.VarChar);
                command.Parameters.Add("@Value", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Type"].Value = periodicZone.Type;
                command.Parameters["@Value"].Value = periodicZone.Value;
                command.Parameters["@Name"].Value = periodicZone.Name;
                command.Parameters["@Period"].Value = periodicZone.Period;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicZone(PeriodicZone periodicZone)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = '{1}' " +
                                         "AND Periodo = {2} ",
                                         table, periodicZone.Name, periodicZone.Period);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


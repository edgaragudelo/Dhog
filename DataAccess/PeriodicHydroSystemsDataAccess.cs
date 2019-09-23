using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicHydroSystemsDataAccess
    {
        public static string table = "sistemaHidroPeriodo";

        public static List<PeriodicHydroSystem> GetPeriodicHydroSystems()
        {
            List<PeriodicHydroSystem> periodicHydroSystems = new List<PeriodicHydroSystem>();

            string query = String.Format("SELECT sistema, Periodo, turbinamientoMinimo, turbinamientoMaximo " +
                           "FROM {0} " +
                           "ORDER BY sistema, Periodo ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicHydroSystems.Add(new PeriodicHydroSystem(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return periodicHydroSystems;
        }

        public static void UpdatePeriodicHydroSystem(PeriodicHydroSystem periodicHydroSystem)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "turbinamientoMinimo = @MinTurbinedOutflow, " +
                                         "turbinamientoMaximo = @MaxTurbinedOutflow " +
                                         "WHERE sistema = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@MinTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@MaxTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@MinTurbinedOutflow"].Value = periodicHydroSystem.MinTurbinedOutflow;
                command.Parameters["@MaxTurbinedOutflow"].Value = periodicHydroSystem.MaxTurbinedOutflow;
                command.Parameters["@Name"].Value = periodicHydroSystem.Name;
                command.Parameters["@Period"].Value = periodicHydroSystem.Period;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicHydroSystem(PeriodicHydroSystem periodicHydroSystem)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE sistema = '{1}' " +
                                         "AND Periodo = {2} ",
                                         table, periodicHydroSystem.Name, periodicHydroSystem.Period);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


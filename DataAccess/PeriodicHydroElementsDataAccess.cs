using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicHydroElementsDataAccess
    {
        public static string table = "elementoHidraulicoPeriodo";

        public static List<PeriodicHydroElement> GetPeriodicHydroElements()
        {
            List<PeriodicHydroElement> periodicHydroElements = new List<PeriodicHydroElement>();

            string query = String.Format("SELECT Nombre, Periodo, TurMinimo, TurMaximo, Filtracion, FactorRecuperacion " +
                           "FROM {0} " +
                           "ORDER BY Nombre, Periodo ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicHydroElements.Add(new PeriodicHydroElement(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4)), Convert.ToDouble(reader.GetValue(5))));

            DataBaseManager.DbConnection.Close();
            
            return periodicHydroElements;
        }

        public static void UpdatePeriodicHydroElement(PeriodicHydroElement periodicHydroElement)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "TurMinimo = @MinTurbinedOutflow, " +
                                         "TurMaximo = @MaxTurbinedOutflow, " +
                                         "Filtracion = @Filtration, " +
                                         "FactorRecuperacion = @RecoveryFactor " +
                                         "WHERE Nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@MinTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@MaxTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@Filtration", OleDbType.Numeric);
                command.Parameters.Add("@RecoveryFactor", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@MinTurbinedOutflow"].Value = periodicHydroElement.MinTurbinedOutflow;
                command.Parameters["@MaxTurbinedOutflow"].Value = periodicHydroElement.MaxTurbinedOutflow;
                command.Parameters["@Filtration"].Value = periodicHydroElement.Filtration;
                command.Parameters["@RecoveryFactor"].Value = periodicHydroElement.RecoveryFactor;
                command.Parameters["@Name"].Value = periodicHydroElement.Name;
                command.Parameters["@Period"].Value = periodicHydroElement.Period;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicHydroElement(PeriodicHydroElement periodicHydroElement)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Nombre = '{1}' " +
                                         "AND Periodo = {2} ",
                                         table, periodicHydroElement.Name, periodicHydroElement.Period);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


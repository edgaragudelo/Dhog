using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicReservoirsDataAccess
    {
        public static string table = "embalsePeriodo";

        public static List<PeriodicReservoir> GetPeriodicReservoirs()
        {
            List<PeriodicReservoir> periodicReservoirs = new List<PeriodicReservoir>();

            string query = String.Format("SELECT Nombre, Periodo, VolumenMinimo, VolumenMaximo, Escenario " +
                           "FROM {0} " +
                           "ORDER BY Nombre, Periodo, Escenario ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicReservoirs.Add(new PeriodicReservoir(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4))));
                
            DataBaseManager.DbConnection.Close();

            return periodicReservoirs;
        }

        public static void UpdatePeriodicReservoir(PeriodicReservoir periodicReservoir)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "VolumenMinimo = @MinLevel, " +
                                         "VolumenMaximo = @MaxLevel " +
                                         "WHERE Nombre = @Name AND " +
                                         "periodo = @Period AND " +
                                         "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@MinLevel", OleDbType.Numeric);
                command.Parameters.Add("@MaxLevel", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@MinLevel"].Value = periodicReservoir.MinLevel;
                command.Parameters["@MaxLevel"].Value = periodicReservoir.MaxLevel;
                command.Parameters["@Name"].Value = periodicReservoir.Name;
                command.Parameters["@Period"].Value = periodicReservoir.Period;
                command.Parameters["@Case"].Value = periodicReservoir.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicReservoir(PeriodicReservoir periodicReservoir)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Nombre = '{1}' " +
                                         "AND Periodo = {2} " +
                                         "AND Escenario = {3}",
                                         table, periodicReservoir.Name, periodicReservoir.Period, periodicReservoir.Case);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


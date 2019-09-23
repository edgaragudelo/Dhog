using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicFuelsDataAccess
    {
        public static string table = "combustiblePeriodo";

        public static List<PeriodicFuel> GetPeriodicFuels()
        {
            List<PeriodicFuel> periodicFuels = new List<PeriodicFuel>();

            string query = String.Format("SELECT CentroAbastecimiento, Periodo, CapacidadHora, MinimoHora, CostoCombustible, CostoTransporte, Escenario " +
                           "FROM {0} " +
                           "ORDER BY CentroAbastecimiento, Periodo, Escenario ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicFuels.Add(new PeriodicFuel(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4)), Convert.ToDouble(reader.GetValue(5)), Convert.ToInt32(reader.GetValue(6))));
                
            DataBaseManager.DbConnection.Close();

            return periodicFuels;
        }

        public static void UpdatePeriodicFuel(PeriodicFuel periodicFuel)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "CapacidadHora = @Capacity, " +
                                         "MinimoHora = @Min, " +
                                         "CostoCombustible = @Cost, " +
                                         "CostoTransporte = @TransportCost " +
                                         "WHERE CentroAbastecimiento = @Name AND " +
                                         "periodo = @Period AND " +
                                         "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Capacity", OleDbType.Numeric);
                command.Parameters.Add("@Min", OleDbType.Numeric);
                command.Parameters.Add("@Cost", OleDbType.Numeric);
                command.Parameters.Add("@TransportCost", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Capacity"].Value = periodicFuel.Capacity;
                command.Parameters["@Min"].Value = periodicFuel.Min;
                command.Parameters["@Cost"].Value = periodicFuel.Cost;
                command.Parameters["@TransportCost"].Value = periodicFuel.TransportCost;
                command.Parameters["@Name"].Value = periodicFuel.Name;
                command.Parameters["@Period"].Value = periodicFuel.Period;
                command.Parameters["@Case"].Value = periodicFuel.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicFuel(PeriodicFuel periodicFuel)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE CentroAbastecimiento = '{1}' " +
                                         "AND Periodo = {2} " +
                                         "AND Escenario = {3}",
                                         table, periodicFuel.Name, periodicFuel.Period, periodicFuel.Case);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


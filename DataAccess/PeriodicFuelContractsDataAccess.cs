using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicFuelContractsDataAccess
    {
        public static string table = "ContratoCombustiblePeriodo";

        public static List<PeriodicFuelContract> GetPeriodicFuelContracts()
        {
            List<PeriodicFuelContract> periodicFuelContracts = new List<PeriodicFuelContract>();

            string query = String.Format("SELECT Nombre, Periodo, CapacidadHora, MinimoHora, CostoContrato, Escenario " +
                           "FROM {0} " +
                           "ORDER BY Nombre, Periodo, Escenario ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicFuelContracts.Add(new PeriodicFuelContract(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4)), Convert.ToInt32(reader.GetValue(5))));

            DataBaseManager.DbConnection.Close();
            
            return periodicFuelContracts;
        }

        public static void UpdatePeriodicFuelContract(PeriodicFuelContract periodicFuelContract)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "CapacidadHora = @Capacity, " +
                                         "MinimoHora = @Min, " +
                                         "CostoContrato = @Cost " +
                                         "WHERE Nombre = @Name AND " +
                                         "periodo = @Period AND " +
                                         "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Capacity", OleDbType.Numeric);
                command.Parameters.Add("@Min", OleDbType.Numeric);
                command.Parameters.Add("@Cost", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Capacity"].Value = periodicFuelContract.Capacity;
                command.Parameters["@Min"].Value = periodicFuelContract.Min;
                command.Parameters["@Cost"].Value = periodicFuelContract.Cost;
                command.Parameters["@Name"].Value = periodicFuelContract.Name;
                command.Parameters["@Period"].Value = periodicFuelContract.Period;
                command.Parameters["@Case"].Value = periodicFuelContract.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicFuelContract(PeriodicFuelContract periodicFuelContract)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Nombre = '{1}' " +
                                         "AND Periodo = {2} " +
                                         "AND Escenario = {3}",
                                         table, periodicFuelContract.Name, periodicFuelContract.Period, periodicFuelContract.Case);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


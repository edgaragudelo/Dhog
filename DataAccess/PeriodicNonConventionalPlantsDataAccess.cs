using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicNonConventionalPlantsDataAccess
    {
        public static string table = "recursoNoCoPeriodo";

        public static List<PeriodicNonConventionalPlant> GetPeriodicNonConventionalPlants()
        {
            List<PeriodicNonConventionalPlant> periodicNonConventionalPlants = new List<PeriodicNonConventionalPlant>();

            string query = String.Format("SELECT Nombre, Periodo, Maximo, FactorPlanta, Escenario " +
                           "FROM {0} " +
                           "ORDER BY Nombre, Periodo, Escenario ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicNonConventionalPlants.Add(new PeriodicNonConventionalPlant(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4))));
                
            DataBaseManager.DbConnection.Close();

            return periodicNonConventionalPlants;
        }

        public static void UpdatePeriodicNonConventionalPlant(PeriodicNonConventionalPlant periodicNonConventionalPlant)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Maximo = @Max, " +
                                         "FactorPlanta = @PlantFactor " +
                                         "WHERE Nombre = @Name AND " +
                                         "periodo = @Period AND " +
                                         "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Max", OleDbType.Numeric);
                command.Parameters.Add("@PlantFactor", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Max"].Value = periodicNonConventionalPlant.Max;
                command.Parameters["@PlantFactor"].Value = periodicNonConventionalPlant.PlantFactor;
                command.Parameters["@Name"].Value = periodicNonConventionalPlant.Name;
                command.Parameters["@Period"].Value = periodicNonConventionalPlant.Period;
                command.Parameters["@Case"].Value = periodicNonConventionalPlant.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicNonConventionalPlant(PeriodicNonConventionalPlant periodicNonConventionalPlant)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Nombre = '{1}' " +
                                         "AND Periodo = {2} " +
                                         "AND Escenario = {3}",
                                         table, periodicNonConventionalPlant.Name, periodicNonConventionalPlant.Period, periodicNonConventionalPlant.Case);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


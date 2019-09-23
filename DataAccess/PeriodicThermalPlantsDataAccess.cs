using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicThermalPlantsDataAccess
    {
        public static string table = "recursoTermicoPeriodo";

        public static List<PeriodicConventionalPlant> GetPeriodicThermalPlants()
        {
            List<PeriodicConventionalPlant> periodicThermalPlants = new List<PeriodicConventionalPlant>();

            string query = String.Format("SELECT Nombre, Periodo, CostoVariable, Minimo, Maximo, Obligatorio, Escenario " +
                           "FROM {0} " +
                           "ORDER BY Nombre, Periodo, Escenario ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicThermalPlants.Add(new PeriodicConventionalPlant(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4)), Convert.ToInt32(reader.GetValue(5)), Convert.ToInt32(reader.GetValue(6))));

            DataBaseManager.DbConnection.Close();
            
            return periodicThermalPlants;
        }

        public static void UpdatePeriodicThermalPlant(PeriodicConventionalPlant periodicThermalPlant)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "CostoVariable = @VariableCost, " +
                                         "Minimo = @Min, " +
                                         "Maximo = @Max, " +
                                         "Obligatorio = @Mandatory " +
                                         "WHERE Nombre = @Name AND " +
                                         "periodo = @Period AND " +
                                         "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@VariableCost", OleDbType.Numeric);
                command.Parameters.Add("@Min", OleDbType.Numeric);
                command.Parameters.Add("@Max", OleDbType.Numeric);
                command.Parameters.Add("@Mandatory", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@VariableCost"].Value = periodicThermalPlant.VariableCost;
                command.Parameters["@Min"].Value = periodicThermalPlant.Min;
                command.Parameters["@Max"].Value = periodicThermalPlant.Max;
                command.Parameters["@Mandatory"].Value = periodicThermalPlant.IsMandatory;
                command.Parameters["@Name"].Value = periodicThermalPlant.Name;
                command.Parameters["@Period"].Value = periodicThermalPlant.Period;
                command.Parameters["@Case"].Value = periodicThermalPlant.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicThermalPlant(PeriodicConventionalPlant periodicThermalPlant)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Nombre = '{1}' " +
                                         "AND Periodo = {2} ",
                                         table, periodicThermalPlant.Name, periodicThermalPlant.Period);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


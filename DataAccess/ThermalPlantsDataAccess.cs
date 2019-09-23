using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class ThermalPlantsDataAccess
    {
        private static string table = "recursoTermicoBasica";

        public static List<ThermalPlant> GetObjects()
        {
            List<ThermalPlant> plants = new List<ThermalPlant>();

            string query = string.Format("SELECT nombre, Combustible, FactorDisponibilidad, FactorConsumoPromedio, Minimo, Maximo, CostoVariable, FactorConsumoVariable, Obligatorio, empresa, EtapaEntrada, Escenario, Id, Subarea " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new ThermalPlant()
                {
                    Name = reader.GetString(0),
                    Fuel = reader.GetString(1),
                    AvailabilityFactor = Convert.ToDouble(reader.GetValue(2)),
                    ProductionFactor = Convert.ToDouble(reader.GetValue(3)),
                    Min = Convert.ToDouble(reader.GetValue(4)),
                    Max = Convert.ToDouble(reader.GetValue(5)),
                    VariableCost = Convert.ToDouble(reader.GetValue(6)),
                    HasVariableProductionFactor = Convert.ToDouble(reader.GetValue(7)),
                    IsMandatory = Convert.ToInt32(reader.GetValue(8)),
                    Company = reader.GetString(9),
                    StartPeriod = Convert.ToInt32(reader.GetValue(10)),
                    Case = Convert.ToInt32(reader.GetValue(11)),
                    Id = Convert.ToInt32(reader.GetValue(12)),
                    Subarea = reader.GetString(13)
                });
            }
            DataBaseManager.DbConnection.Close();

            return plants;
        }

        public static int UpdateObject(ThermalPlant dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(nombre, Combustible, FactorDisponibilidad, FactorConsumoPromedio, Minimo, Maximo, CostoVariable, FactorConsumoVariable, Obligatorio, empresa, EtapaEntrada, Escenario, Subarea) " +
                                        "VALUES(@Name, @Fuel, @AvailabilityFactor, @ProductionFactor, @Min, @Max, @VariableCost, @VariableProductionFactor, @Mandatory, @Company, @StartPeriod, @Case, @Subarea)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "Combustible = @Fuel, " +
                                        "FactorDisponibilidad = @AvailabilityFactor, " +
                                        "FactorConsumoPromedio = @ProductionFactor, " +
                                        "Minimo = @Min, " +
                                        "Maximo = @Max, " +
                                        "CostoVariable = @VariableCost, " +
                                        "FactorConsumoVariable = @VariableProductionFactor, " +
                                        "Obligatorio = @Mandatory, " +
                                        "empresa = @Company, " +
                                        "EtapaEntrada = @StartPeriod, " +
                                        "escenario = @Case, " +
                                        "Subarea = @Subarea " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Fuel", OleDbType.VarChar);
                command.Parameters.Add("@AvailabilityFactor", OleDbType.Numeric);
                command.Parameters.Add("@ProductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Min", OleDbType.Numeric);
                command.Parameters.Add("@Max", OleDbType.Numeric);
                command.Parameters.Add("@VariableCost", OleDbType.Numeric);
                command.Parameters.Add("@VariableProductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Mandatory", OleDbType.Numeric);
                command.Parameters.Add("@Company", OleDbType.VarChar);
                command.Parameters.Add("@StartPeriod", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Subarea", OleDbType.VarChar);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Fuel"].Value = dataObject.Fuel;
                command.Parameters["@AvailabilityFactor"].Value = dataObject.AvailabilityFactor;
                command.Parameters["@ProductionFactor"].Value = dataObject.ProductionFactor;
                command.Parameters["@Min"].Value = dataObject.Min;
                command.Parameters["@Max"].Value = dataObject.Max;
                command.Parameters["@VariableCost"].Value = dataObject.VariableCost;
                command.Parameters["@VariableProductionFactor"].Value = dataObject.HasVariableProductionFactor; 
                command.Parameters["@Mandatory"].Value = dataObject.IsMandatory;
                command.Parameters["@Company"].Value = dataObject.Company;
                command.Parameters["@StartPeriod"].Value = dataObject.StartPeriod;
                command.Parameters["@Case"].Value = dataObject.Case;
                command.Parameters["@Subarea"].Value = dataObject.Subarea;
                command.Parameters["@Id"].Value = dataObject.Id;

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                }
                catch
                {
                    DataBaseManager.DbConnection.Close();
                    throw;
                }
                DataBaseManager.DbConnection.Close();
            }

            if (isNew)
            {
                int id;
                query = string.Format("SELECT Max(Id) FROM {0}", table);
                reader = DataBaseManager.ReadData(query);
                reader.Read();
                id = Convert.ToInt32(reader.GetValue(0));
                DataBaseManager.DbConnection.Close();
                return id;
            }
            else
                return -1;
        }

        public static void DeleteObject(ThermalPlant dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

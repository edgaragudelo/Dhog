using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;
using Telerik.Windows.Controls;

namespace DHOG_WPF.DataAccess
{
    public class HydroPlantsDataAccess
    {
        private static string table = "recursoHidroBasica";

        public static List<HydroPlant> GetObjects()
        {

            string vble = null;
            string vble1 = null;
            List<HydroPlant> plants = new List<HydroPlant>();

            string query = string.Format("SELECT nombre, FactorDisponibilidad, FactorConversionPromedio, Minimo, Maximo, CostoVariable, PorcentajeAGC, FactorConversionVariable, Obligatorio, empresa, EtapaEntrada, Escenario, Id, Subarea " +
                                         "FROM {0} " + "ORDER BY Escenario, Nombre",
                                         table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {

               
                try
                {
                    vble = null;
                    vble1 = null;
                    if (!reader.IsDBNull(13))
                        vble = reader.GetString(13);
                    else vble = string.Empty;
                    if (!reader.IsDBNull(1))
                        vble1 = reader.GetString(0);
                    else vble1 = string.Empty;


                    plants.Add(new HydroPlant()
                {
                    Name = vble1 , //reader.GetString(0),
                    AvailabilityFactor = Convert.ToDouble(reader.GetValue(1)),
                    ProductionFactor = Convert.ToDouble(reader.GetValue(2)),
                    Min = Convert.ToDouble(reader.GetValue(3)),
                    Max = Convert.ToDouble(reader.GetValue(4)),
                    VariableCost = Convert.ToDouble(reader.GetValue(5)),
                    AGCPercentage = Convert.ToDouble(reader.GetValue(6)),
                    HasVariableProductionFactor = Convert.ToDouble(reader.GetValue(7)),
                    IsMandatory = Convert.ToInt32(reader.GetValue(8)),
                    Company = reader.GetString(9),
                    StartPeriod = Convert.ToInt32(reader.GetValue(10)),
                    Case = Convert.ToInt32(reader.GetValue(11)),
                    Id = Convert.ToInt32(reader.GetValue(12)),
                    Subarea = vble //reader.GetString(13)             
                });
                }
                catch (Exception Ex)
                {
                    RadWindow.Alert(new DialogParameters
                    {
                        Content = Ex.Message,
                        //Content = MessageUtil.FormatMessage("FATAL.DBConnectionError"),
                        //Owner = this
                    });
                    //DBConversionBusyIndicator.IsBusy = false;

                   
                }

               
                
            }
            DataBaseManager.DbConnection.Close();
            
            return plants;
        }

        public static int UpdateObject(HydroPlant dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(nombre, FactorDisponibilidad, FactorConversionPromedio, Minimo, Maximo, CostoVariable, PorcentajeAGC, FactorConversionVariable, Obligatorio, empresa, EtapaEntrada, Escenario, Subarea) " +
                                        "VALUES(@Name, @AvailabilityFactor, @ProductionFactor, @Min, @Max, @VariableCost, @AGCPercentage, @VariableProductionFactor, @Mandatory, @Company, @StartPeriod, @Case, @Subarea)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "FactorDisponibilidad = @AvailabilityFactor, " +
                                        "FactorConversionPromedio = @ProductionFactor, " +
                                        "Minimo = @Min, " +
                                        "Maximo = @Max, " +
                                        "CostoVariable = @VariableCost, " +
                                        "PorcentajeAGC = @AGCPercentage, " +
                                        "FactorConversionVariable = @VariableProductionFactor, " +
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
                command.Parameters.Add("@AvailabilityFactor", OleDbType.Numeric);
                command.Parameters.Add("@ProductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Min", OleDbType.Numeric);
                command.Parameters.Add("@Max", OleDbType.Numeric);
                command.Parameters.Add("@VariableCost", OleDbType.Numeric);
                command.Parameters.Add("@AGCPercentage", OleDbType.Numeric);
                command.Parameters.Add("@VariableProductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Mandatory", OleDbType.Numeric);
                command.Parameters.Add("@Company", OleDbType.VarChar);
                command.Parameters.Add("@StartPeriod", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Subarea", OleDbType.VarChar);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@AvailabilityFactor"].Value = dataObject.AvailabilityFactor;
                command.Parameters["@ProductionFactor"].Value = dataObject.ProductionFactor;
                command.Parameters["@Min"].Value = dataObject.Min;
                command.Parameters["@Max"].Value = dataObject.Max;
                command.Parameters["@VariableCost"].Value = dataObject.VariableCost;
                command.Parameters["@AGCPercentage"].Value = dataObject.AGCPercentage;
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
        

        public static void DeleteObject(HydroPlant dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


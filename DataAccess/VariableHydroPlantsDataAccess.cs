using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class VariableHydroPlantsDataAccess
    {
        private static string table = "recursoHidroVariable";

        public static List<VariableHydroPlant> GetObjects()
        {
            List<VariableHydroPlant> plants = new List<VariableHydroPlant>();

            string query = string.Format("SELECT Recurso, Embalse, Segmento, Volumen, FactorConversion, GeneracionMaxima, Escenario, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new VariableHydroPlant()
                {
                    Name = reader.GetString(0),
                    Reservoir = reader.GetString(1),
                    Segment = Convert.ToInt32(reader.GetValue(2)),
                    Level = Convert.ToDouble(reader.GetValue(3)),
                    ProductionFactor = Convert.ToDouble(reader.GetValue(4)),
                    Max = Convert.ToDouble(reader.GetValue(5)),
                    Case = Convert.ToInt32(reader.GetValue(6)),
                    Id = Convert.ToInt32(reader.GetValue(7))
                });
            }
            DataBaseManager.DbConnection.Close();
            
            return plants;
        }

        public static int UpdateObject(VariableHydroPlant dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT Recurso " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Recurso, Embalse, Segmento, Volumen, FactorConversion, GeneracionMaxima, Escenario) " +
                                        "VALUES(@Name, @Reservoir, @Segment, @Level, @ProductionFactor, @Max, @Case)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Recurso = @Name, " +
                                        "Embalse = @Reservoir, " +
                                        "Segmento = @Segment, " +
                                        "Volumen = @Level, " +
                                        "FactorConversion = @ProductionFactor, " +
                                        "GeneracionMaxima = @Max, " +
                                        "escenario = @Case " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Reservoir", OleDbType.VarChar);
                command.Parameters.Add("@Segment", OleDbType.Numeric);
                command.Parameters.Add("@Level", OleDbType.Numeric);
                command.Parameters.Add("@ProductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Max", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Reservoir"].Value = dataObject.Reservoir;
                command.Parameters["@Segment"].Value = dataObject.Segment;
                command.Parameters["@Level"].Value = dataObject.Level;
                command.Parameters["@ProductionFactor"].Value = dataObject.ProductionFactor;
                command.Parameters["@Max"].Value = dataObject.Max;
                command.Parameters["@Case"].Value = dataObject.Case;
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
        

        public static void DeleteObject(VariableHydroPlant dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


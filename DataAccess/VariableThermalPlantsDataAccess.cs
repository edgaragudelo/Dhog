using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class VariableThermalPlantsDataAccess
    {
        private static string table = "recursoTermicoVariable";

        public static List<VariableConventionalPlant> GetObjects()
        {
            List<VariableConventionalPlant> plants = new List<VariableConventionalPlant>();

            string query = string.Format("SELECT Recurso, Segmento, FactorConsumo, GeneracionMaxima, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new VariableConventionalPlant()
                {
                    Name = reader.GetString(0),
                    Segment = Convert.ToInt32(reader.GetValue(1)),
                    ProductionFactor = Convert.ToDouble(reader.GetValue(2)),
                    Max = Convert.ToDouble(reader.GetValue(3)),
                    Id = Convert.ToInt32(reader.GetValue(4))
                });
            }
            DataBaseManager.DbConnection.Close();
            
            return plants;
        }

        public static int UpdateObject(VariableConventionalPlant dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT Recurso " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Recurso, Segmento, FactorConsumo, GeneracionMaxima) " +
                                        "VALUES(@Name, @Segment, @ProductionFactor, @Max)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Recurso = @Name, " +
                                        "Segmento = @Segment, " +
                                        "FactorConsumo = @ProductionFactor, " +
                                        "GeneracionMaxima = @Max " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Segment", OleDbType.Numeric);
                command.Parameters.Add("@ProductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Max", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Segment"].Value = dataObject.Segment;
                command.Parameters["@ProductionFactor"].Value = dataObject.ProductionFactor;
                command.Parameters["@Max"].Value = dataObject.Max;
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
        

        public static void DeleteObject(VariableConventionalPlant dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


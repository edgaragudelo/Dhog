using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class NonConventionalPlantsDataAccess
    {
        private static string table = "recursoNoCoBasica";

        public static List<NonConventionalPlant> GetObjects()
        {
            List<NonConventionalPlant> plants = new List<NonConventionalPlant>();

            string query = string.Format("SELECT nombre, Tipo, FactorPlanta, Maximo, empresa, EtapaEntrada, Escenario, Id, Subarea " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new NonConventionalPlant()
                {
                    Name = reader.GetString(0),
                    Type = reader.GetString(1),
                    ProductionFactor = Convert.ToDouble(reader.GetValue(2)),
                    Max = Convert.ToDouble(reader.GetValue(3)),
                    Company = reader.GetString(4),
                    StartPeriod = Convert.ToInt32(reader.GetValue(5)),
                    Case = Convert.ToInt32(reader.GetValue(6)),
                    Id = Convert.ToInt32(reader.GetValue(7)),
                    Subarea = reader.GetString(8)
                });
            }
            DataBaseManager.DbConnection.Close();

            return plants;
        }

        public static int UpdateObject(NonConventionalPlant dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(nombre, Tipo, FactorPlanta, Maximo, empresa, EtapaEntrada, Escenario, Subarea) " +
                                        "VALUES(@Name, @Type, @ProductionFactor, @Max, @Company, @StartPeriod, @Case, @Subarea)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "Tipo = @Type, " +
                                        "FactorPlanta = @ProductionFactor, " +
                                        "Maximo = @Max, " +
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
                command.Parameters.Add("@Type", OleDbType.VarChar);
                command.Parameters.Add("@ProductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Max", OleDbType.Numeric);
                command.Parameters.Add("@Company", OleDbType.VarChar);
                command.Parameters.Add("@StartPeriod", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Subarea", OleDbType.VarChar);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Type"].Value = dataObject.Type;
                command.Parameters["@ProductionFactor"].Value = dataObject.ProductionFactor;
                command.Parameters["@Max"].Value = dataObject.Max;
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

        public static void DeleteObject(NonConventionalPlant dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class FuelContractsDataAccess
    {
        private static string table = "ContratoCombustibleBasica";

        public static List<FuelContract> GetObjects()
        {
            List<FuelContract> plants = new List<FuelContract>();

            string query = string.Format("SELECT Nombre, Tipo, CapacidadHora, MinimoHora, CostoContrato, EtapaInicial, EtapaFinal, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new FuelContract()
                {
                    Name = reader.GetString(0),
                    Type = reader.GetString(1),
                    Capacity = Convert.ToDouble(reader.GetValue(2)),
                    Min = Convert.ToDouble(reader.GetValue(3)),
                    Cost = Convert.ToDouble(reader.GetValue(4)),
                    InitialPeriod = Convert.ToInt32(reader.GetValue(5)),
                    FinalPeriod= Convert.ToInt32(reader.GetValue(6)),
                    Id = Convert.ToInt32(reader.GetValue(7))
                });
            }
            DataBaseManager.DbConnection.Close();

            return plants;
        }

        public static int UpdateObject(FuelContract dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT Nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Nombre, Tipo, CapacidadHora, MinimoHora, CostoContrato, EtapaInicial, EtapaFinal) " +
                                        "VALUES(@Name, @Type, @Capacity, @Min, @Cost, @InitialPeriod, @FinalPeriod)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Nombre = @Name, " +
                                        "Tipo = @Type, " +
                                        "CapacidadHora = @Capacity, " +
                                        "MinimoHora = @Min, " +
                                        "CostoContrato = @Cost, " +
                                        "EtapaInicial = @InitialPeriod, " +
                                        "EtapaFinal = @FinalPeriod " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Type", OleDbType.VarChar);
                command.Parameters.Add("@Capacity", OleDbType.Numeric);
                command.Parameters.Add("@Min", OleDbType.Numeric);
                command.Parameters.Add("@Cost", OleDbType.Numeric);
                command.Parameters.Add("@InitialPeriod", OleDbType.Numeric);
                command.Parameters.Add("@FinalPeriod", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Type"].Value = dataObject.Type;
                command.Parameters["@Capacity"].Value = dataObject.Capacity;
                command.Parameters["@Min"].Value = dataObject.Min;
                command.Parameters["@Cost"].Value = dataObject.Cost;
                command.Parameters["@InitialPeriod"].Value = dataObject.InitialPeriod;
                command.Parameters["@FinalPeriod"].Value = dataObject.FinalPeriod;
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

        public static void DeleteObject(FuelContract dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

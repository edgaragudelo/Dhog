using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class FuelsDataAccess
    {
        private static string table = "combustibleBasica";

        public static List<Fuel> GetObjects()
        {
            List<Fuel> plants = new List<Fuel>();

            string query = string.Format("SELECT CentroAbastecimiento, Tipo, CapacidadHora, MinimoHora, CostoCombustible, CostoTransporte, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new Fuel()
                {
                    Name = reader.GetString(0),
                    Type = reader.GetString(1),
                    Capacity = Convert.ToDouble(reader.GetValue(2)),
                    Min = Convert.ToDouble(reader.GetValue(3)),
                    Cost = Convert.ToDouble(reader.GetValue(4)),
                    TransportCost = Convert.ToDouble(reader.GetValue(5)),
                    Id = Convert.ToInt32(reader.GetValue(6))
                });
            }
            DataBaseManager.DbConnection.Close();
            
            return plants;
        }

        public static int UpdateObject(Fuel dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT CentroAbastecimiento " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(CentroAbastecimiento, Tipo, CapacidadHora, MinimoHora, CostoCombustible, CostoTransporte) " +
                                        "VALUES(@Name, @Type, @Capacity, @Min, @Cost, @TransportCost)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "CentroAbastecimiento = @Name, " +
                                        "Tipo = @Type, " +
                                        "CapacidadHora = @Capacity, " +
                                        "MinimoHora = @Min, " +
                                        "CostoCombustible = @Cost, " +
                                        "CostoTransporte = @TransportCost " +
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
                command.Parameters.Add("@TransportCost", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Type"].Value = dataObject.Type;
                command.Parameters["@Capacity"].Value = dataObject.Capacity;
                command.Parameters["@Min"].Value = dataObject.Min;
                command.Parameters["@Cost"].Value = dataObject.Cost;
                command.Parameters["@TransportCost"].Value = dataObject.TransportCost;
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

        public static void DeleteObject(Fuel dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

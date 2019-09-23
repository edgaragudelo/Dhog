using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class BlocksDataAccess
    {
        private static string table = "BloqueBasica";

        public static List<Block> GetObjects()
        {
            List<Block> blocks = new List<Block>();

            string query = string.Format("SELECT nombre, FactorDuracion, FactorDemanda, Id " +
                                         "FROM {0} ORDER BY Nombre", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                blocks.Add(new Block(Convert.ToInt32(reader.GetValue(0)), Convert.ToDouble(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3))));
            
            DataBaseManager.DbConnection.Close();
            return blocks;
        }

        public static int UpdateObject(Block dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(nombre, FactorDuracion, FactorDemanda) " +
                                        "VALUES(@Name, @DurationFactor, @LoadFactor)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "FactorDuracion = @DurationFactor, " +
                                        "FactorDemanda = @LoadFactor " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@DurationFactor", OleDbType.Numeric);
                command.Parameters.Add("@LoadFactor", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@DurationFactor"].Value = dataObject.DurationFactor;
                command.Parameters["@LoadFactor"].Value = dataObject.LoadFactor;
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

        public static void DeleteObject(Block dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

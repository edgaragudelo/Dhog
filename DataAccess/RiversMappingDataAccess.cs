using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class RiversMappingDataAccess
    {
        private static string table = "RioBasica";
        //private static string table = "MapeoRios";

        public static List<NameMapping> GetObjects()
        {
            List<NameMapping> namesMapping = new List<NameMapping>();

            string query = string.Format("SELECT Rio, Id " +
                                         "FROM {0} " +
                                         "ORDER BY id", table);

            //string query = string.Format("SELECT Rio, Numero " +
            //                             "FROM {0} " +
            //                             "ORDER BY Numero", table);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                namesMapping.Add(new NameMapping(reader.GetString(0), Convert.ToInt32(reader.GetValue(1))));

            DataBaseManager.DbConnection.Close();
            return namesMapping;
        }

        public static void UpdateObject(NameMapping dataObject)
        {
            string query = string.Format("SELECT Rio " +
                                         "FROM {0} " +
                                         "WHERE Rio = '{1}'", table, dataObject.DHOGName);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
                query = string.Format("INSERT INTO {0}(Id, Rio) " +
                                        "VALUES(@SDDPNumber, @DHOGName)", table);
            //query = string.Format("INSERT INTO {0}(Numero, Rio) " +
            //                          "VALUES(@SDDPNumber, @DHOGName)", table);
            
            else
                query = string.Format("UPDATE {0} SET " +
                                        "Id = @SDDPNumber " +
                                        "WHERE Rio = @DHOGName", table);

            //query = string.Format("UPDATE {0} SET " +
            //                            "Numero = @SDDPNumber " +
            //                            "WHERE Rio = @DHOGName", table);

            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@SDDPNumber", OleDbType.VarChar);
                command.Parameters.Add("@DHOGName", OleDbType.VarChar);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@SDDPNumber"].Value = dataObject.SDDPNumber;
                command.Parameters["@DHOGName"].Value = dataObject.DHOGName;

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
        }

        public static void DeleteObject(NameMapping dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Rio = '{1}'", table, dataObject.DHOGName);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

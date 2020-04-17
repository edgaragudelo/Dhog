using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class ReservoirsMappingDataAccess
    {
        private static string table = "MapeoEmbalses";
        //private static string table = "EmbalseBasica";

        public static List<NameMapping> GetObjects()
        {
            List<NameMapping> namesMapping = new List<NameMapping>();

            string query = string.Format("SELECT Embalse, Recurso " +
            //string query = string.Format("SELECT nombre, nombre " +
                                         "FROM {0} " +
                                         "ORDER BY Embalse", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                namesMapping.Add(new NameMapping(reader.GetString(0), reader.GetString(1)));
            
            DataBaseManager.DbConnection.Close();
            return namesMapping;
        }

        public static void UpdateObject(NameMapping dataObject)
        {
            string query = string.Format("SELECT Embalse " +
                                         "FROM {0} " +
                                         "WHERE Embalse = '{1}'", table, dataObject.DHOGName);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
                query = string.Format("INSERT INTO {0}(Recurso, Embalse) " +
                                        "VALUES(@SDDPName, @DHOGName)", table);
            
            else
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @SDDPName " +
                                        "WHERE nombre = @DHOGName", table);
            
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@SDDPName", OleDbType.VarChar);
                command.Parameters.Add("@DHOGName", OleDbType.VarChar);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@SDDPName"].Value = dataObject.SDDPName;
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
                                         "WHERE nombre = '{1}'", table, dataObject.DHOGName);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

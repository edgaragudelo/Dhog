using DHOG_WPF.Models;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class HydroPlantsMappingDataAccess
    {
        //private static string table = "MapeoRecursosHidro";
        private static string table = "RecursoHidroBasica";
        public static List<NameMapping> GetObjects()
        {
            List<NameMapping> namesMapping = new List<NameMapping>();

            //string query = string.Format("SELECT Recurso, Planta " +
            //                             "FROM {0} " +
            //                             "ORDER BY Recurso", table);
            string query = string.Format("SELECT Nombre, Nombre " +
                                        "FROM {0} " +
                                        "ORDER BY Nombre", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                namesMapping.Add(new NameMapping(reader.GetString(0), reader.GetString(1)));
            
            DataBaseManager.DbConnection.Close();
            return namesMapping;
        }

        public static void UpdateObject(NameMapping dataObject)
        {
            string query = string.Format("SELECT Recurso " +
                                         "FROM {0} " +
                                         "WHERE Recurso = '{1}'", table, dataObject.DHOGName);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
                query = string.Format("INSERT INTO {0}(Planta, Recurso) " +
                                        "VALUES(@SDDPName, @DHOGName)", table);
            
            else
                query = string.Format("UPDATE {0} SET " +
                                        "Planta = @SDDPName " +
                                        "WHERE Recurso = @DHOGName", table);
            
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
                                         "WHERE Recurso = '{1}'", table, dataObject.DHOGName);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

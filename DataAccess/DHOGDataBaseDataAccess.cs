using DHOG_WPF.Models;
using log4net;
using System;
using System.Data.OleDb;


namespace DHOG_WPF.DataAccess
{
    public class DHOGDataBaseDataAccess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DHOGDataBaseDataAccess));
        private string dbFile;
        private static string table = "InfoBD";

        public DHOGDataBaseDataAccess(string inputDataSource, string ouputDataSource, string TipobaseDatos)
        {
            dbFile = inputDataSource;            
            DataBaseManager dataBaseManager = new DataBaseManager(inputDataSource, ouputDataSource,TipobaseDatos);
        }

        public static void GetDBInformation(DHOGDataBase dhogCase)
        {
            try
            {
                string query = string.Format("SELECT Descripcion, VersionDHOG " +
                                             "FROM {0} WHERE Id = 1", table);
                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    DataBaseManager.DbConnection.Open();
                    try
                    {
                        OleDbDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            dhogCase.Description = reader.GetString(0);
                            dhogCase.Version = Convert.ToDouble(reader.GetValue(1));
                        }
                    }
                    catch (Exception Ex)
                    {
                        dhogCase.Version = 0;
                        DataBaseManager.DbConnection.Close();
                    }

                    DataBaseManager.DbConnection.Close();
                }
            }
            catch (Exception Ex)
            {
                DataBaseManager.DbConnection.Close();
                throw;
            }
        }

        public static void UpdateCaseDescription(string description)
        {
            string query = string.Format("UPDATE {0} " +
                                         "SET Descripcion = '{1}' " +
                                         "WHERE Id = 1", table, description);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

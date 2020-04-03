using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class AreasDataAccess
    {
        private static string table = "areaBasica";

        public static List<Area> GetObjects()
        {
            List<Area> areas = new List<Area>();

            string query = string.Format("SELECT Nombre, factorDemanda, limiteImportacion, limiteExportacion, Id " +
                                         "FROM {0} ORDER BY Nombre", table);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                areas.Add(new Area()
                {
                    Name = reader.GetString(0),
                    BaseLoad = Convert.ToDouble(reader.GetValue(1)),
                    ImportationLimit = Convert.ToDouble(reader.GetValue(2)),
                    ExportationLimit = Convert.ToDouble(reader.GetValue(3)),
                    Id = Convert.ToInt32(reader.GetValue(4))
                });
            }
            DataBaseManager.DbConnection.Close();

            return areas;
        }

        public static int UpdateObject(Area dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(nombre, factorDemanda, limiteImportacion, limiteExportacion) " +
                                        "VALUES(@Name, @BaseLoad, @ImportationLimit, @ExportationLimit)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "factorDemanda = @BaseLoad, " +
                                        "limiteImportacion = @ImportationLimit, " +
                                        "limiteExportacion = @ExportationLimit " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@BaseLoad", OleDbType.Numeric);
                command.Parameters.Add("@ImportationLimit", OleDbType.Numeric);
                command.Parameters.Add("@ExportationLimit", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@BaseLoad"].Value = dataObject.BaseLoad;
                command.Parameters["@ImportationLimit"].Value = dataObject.ImportationLimit;
                command.Parameters["@ExportationLimit"].Value = dataObject.ExportationLimit;
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
                query = string.Format("SELECT Max(Id) FROM {0}", table);
                reader = DataBaseManager.ReadData(query);
                reader.Read();
                int id = Convert.ToInt32(reader.GetValue(0));
                DataBaseManager.DbConnection.Close();
                return id;
            }
            else
                return -1;
        }

        public static void DeleteObject(Area dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class ZonesDataAccess
    {
        private static string table = "ZonaBasica";

        public static List<Zone> GetZones()
        {
            List<Zone> zones = new List<Zone>();

            string query = string.Format("SELECT Nombre, Tipo, Valor, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                zones.Add(new Zone(reader.GetString(0), reader.GetString(1), Convert.ToDouble(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3))));
            
            DataBaseManager.DbConnection.Close();

            foreach (Zone zone in zones)
            {
                List<string> plants = new List<string>();
                query = "SELECT Recurso " +
                        "FROM zonaRecurso " +
                        "WHERE Nombre = '" + zone.Name + "'";
                reader = DataBaseManager.ReadData(query);
                while (reader.Read())
                    plants.Add(reader.GetString(0));

                DataBaseManager.DbConnection.Close();
                
                zone.Plants = plants;
            }

            return zones;
        }

        public static int UpdateZone(Zone dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Valor, Nombre, Tipo) " +
                                        "VALUES(@Value, @Name, @Type)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Valor = @Value, " +
                                        "nombre = @Name, " +
                                        "Tipo = @Type " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Value", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Type", OleDbType.VarChar);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Value"].Value = dataObject.Value;
                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Type"].Value = dataObject.Type;

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

        public static void DeleteZone(Zone dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }

        public static void AddPlantToZone(string zone, string plant)
        {
            string query = string.Format("SELECT * " +
                                         "FROM ZonaRecurso " +
                                         "WHERE nombre = '{0}' AND recurso = '{1}'", zone, plant);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO ZonaRecurso(nombre, recurso) " +
                                        " VALUES('{0}', '{1}')", zone, plant);
            }
            else
                query = null;
            DataBaseManager.DbConnection.Close();

            if (query != null)
                DataBaseManager.ExecuteQuery(query);
        }

        public static void DeletePlantFromZone(string zone, string plant)
        {
            string query = "DELETE FROM ZonaRecurso " +
                           "WHERE nombre = '" + zone + "' " +
                           "AND recurso = '" + plant + "' ";
            DataBaseManager.ExecuteQuery(query);
        }

        public static void DeleteAllPlantsFromZone(string zone)
        {
            string query = "DELETE FROM ZonaRecurso " +
                           "WHERE nombre = '" + zone + "' ";
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


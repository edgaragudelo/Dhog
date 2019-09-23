using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Forms;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class EspecialZonesDataAccess
    {

     
        private static string table = "ZonaEspecial";
       



        public static List<EspecialZone> GetZones()
        {
            List<EspecialZone> zones = new List<EspecialZone>();

            string query = string.Format("SELECT Nombre, IndiceIni, IndiceFin, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                zones.Add(new EspecialZone(reader.GetString(0), Convert.ToDouble(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            string Zonaantes = null;
            foreach (EspecialZone zone in zones)
            {
                if (Zonaantes != zone.Name)
                {
                    List<string> plants = new List<string>();
                    query = "SELECT Recurso " +
                        "FROM zonaRecurso " +
                        "WHERE Nombre = '" + zone.Name + "'";
                    reader = DataBaseManager.ReadData(query);
                    while (reader.Read())
                        plants.Add(reader.GetString(0));

                   
                    zone.Plants = plants;
                    Zonaantes = zone.Name;
                    DataBaseManager.DbConnection.Close();
                }
            }
            
            return zones;
        }


        private static bool EstadoRango(EspecialZone dataobject1, out double IndiceIniantes, out double IndiceFinantes)
        {
            bool State = false;
            IndiceIniantes = 0;
            IndiceFinantes = 0;
            string query = string.Format("SELECT nombre,IndiceIni,IndiceFin " +
                                       "FROM ZonaEspecial " +
                                       "WHERE ((nombre='" + dataobject1.Name + "' and " + dataobject1.IndiceIni +
                                       " between IndiceIni and IndiceFin) or (nombre='" + dataobject1.Name + "' and " +
                                       dataobject1.IndiceFin + " between IndiceIni and IndiceFin))");

            OleDbDataReader reader = DataBaseManager.ReadData(query);



            if (!reader.Read())
                State = false;
            else
            {
                State = true;
                DataBaseManager.DbConnection.Close();
                reader = DataBaseManager.ReadData(query);
                while (reader.Read())
                {
                    IndiceIniantes = Convert.ToDouble(reader.GetValue(1)); IndiceFinantes = Convert.ToDouble(reader.GetValue(2));
                }
            }
            DataBaseManager.DbConnection.Close();
            return State;
        }

      

        public static int UpdateZone(EspecialZone dataObject)
        {
            int id;
            bool isNew = false;
            bool RangosOk;
            string query = null;

            double Iniantes=0, Finantes=0;

            // Validar los rangos de los datos recien ingresados

            RangosOk = EstadoRango(dataObject, out Iniantes, out Finantes);

            if (!RangosOk)
            {
                query = string.Format("INSERT INTO {0}(Nombre, IndiceIni,IndiceFin) " +
                                            "VALUES(@Name, @IndiceIni, @IndiceFin)", table);
                isNew = true;
            }
            else
            {
                var result = System.Windows.Forms.MessageBox.Show("Los intervalos ya existen, Quiere cambiarlos", "Error de Datos",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

                query = null;
                if (result == DialogResult.Yes)
                {
                   query = string.Format("UPDATE {0} SET " +
                   "nombre = @Name, " +
                   "IndiceIni = @IndiceIni, " +
                   "IndiceFin = @IndiceFin " +
                   "WHERE Id = @Id", table);

                }
                else
                {
                    query = string.Format("UPDATE {0} SET " +
                  "nombre = @Name, " +
                  "IndiceIni = @Iniantes, " +
                  "IndiceFin = @Finantes " +
                  "WHERE Id = @Id", table);

                }


                
            }
                DataBaseManager.DbConnection.Close();
            if (query != null)
            {
                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {

                    command.Parameters.Add("@Name", OleDbType.VarChar);
                    command.Parameters.Add("@IndiceIni", OleDbType.Numeric);
                    command.Parameters.Add("@IndiceFin", OleDbType.Numeric);
                    command.Parameters.Add("@Id", OleDbType.Numeric);

                    DataBaseManager.DbConnection.Open();


                    command.Parameters["@Name"].Value = dataObject.Name;
                    command.Parameters["@IndiceIni"].Value = dataObject.IndiceIni;
                    command.Parameters["@IndiceFin"].Value = dataObject.IndiceFin;
                    command.Parameters["@Id"].Value = dataObject.Id;


                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        DataBaseManager.DbConnection.Close();
                        System.Windows.MessageBox.Show("Error", ex.Message);
                        throw;
                    }

                    DataBaseManager.DbConnection.Close();
                }
            }
                if (isNew)
                {
                    DataBaseManager.DbConnection.Close();
                    query = string.Format("SELECT Max(Id) FROM {0}", table);
                    OleDbDataReader reader = DataBaseManager.ReadData(query);
                    //reader = DataBaseManager.ReadData(query);
                    reader.Read();
                    id = Convert.ToInt32(reader.GetValue(0));
                    DataBaseManager.DbConnection.Close();
                    return id;
                }
                else
                    return -1;      
            }
    

        public static void DeleteZone(EspecialZone dataObject)
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


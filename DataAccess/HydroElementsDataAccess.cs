using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class HydroElementsDataAccess
    {
        private static string table = "elementoHidraulicoBasica";

        public static List<HydroElement> GetObjects()
        {
            List<HydroElement> hydroElements = new List<HydroElement>();

            string query = string.Format("SELECT Nombre, TurMinimo, TurMaximo, Filtracion, FactorRecuperacion, EtapaEntrada, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                hydroElements.Add(new HydroElement()
                {
                    Name = reader.GetString(0),
                    MinTurbinedOutflow = Convert.ToDouble(reader.GetValue(1)),
                    MaxTurbinedOutflow = Convert.ToDouble(reader.GetValue(2)),
                    Filtration = Convert.ToInt32(reader.GetValue(3)),
                    RecoveryFactor = Convert.ToDouble(reader.GetValue(4)),
                    StartPeriod = Convert.ToInt32(reader.GetValue(5)),
                    Id = Convert.ToInt32(reader.GetValue(6))
                });
            }
            DataBaseManager.DbConnection.Close();

            return hydroElements;
        }

        public static int UpdateObject(HydroElement dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Nombre, TurMinimo, TurMaximo, Filtracion, FactorRecuperacion, EtapaEntrada) " +
                        "VALUES(@Name, @MinTurbinedOutflow, @MaxTurbinedOutflow, @Filtration, @RecoveryFactor, @InitialPeriod)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Nombre = @Name, " +
                                        "TurMinimo = @MinTurbinedOutflow, " +
                                        "TurMaximo = @MaxTurbinedOutflow, " +
                                        "Filtracion = @Filtration, " +
                                        "FactorRecuperacion = @RecoveryFactor, " +
                                        "EtapaEntrada = @InitialPeriod " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@MinTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@MaxTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@Filtration", OleDbType.Numeric);
                command.Parameters.Add("@RecoveryFactor", OleDbType.Numeric);
                command.Parameters.Add("@InitialPeriod", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@MinTurbinedOutflow"].Value = dataObject.MinTurbinedOutflow;
                command.Parameters["@MaxTurbinedOutflow"].Value = dataObject.MaxTurbinedOutflow;
                command.Parameters["@Filtration"].Value = dataObject.Filtration;
                command.Parameters["@RecoveryFactor"].Value = dataObject.RecoveryFactor;
                command.Parameters["@InitialPeriod"].Value = dataObject.StartPeriod;
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

        public static void DeleteObject(HydroElement dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


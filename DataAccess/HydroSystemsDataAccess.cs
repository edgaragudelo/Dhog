using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class HydroSystemsDataAccess
    {
        private static string table = "sistemaHidroBasica";

        public static List<HydroSystem> GetObjects()
        {
            List<HydroSystem> hydroSystems = new List<HydroSystem>();

            string query = string.Format("SELECT sistema, turbinamientoMinimo, turbinamientoMaximo, FactorEnergia, EtapaEntrada, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                hydroSystems.Add(new HydroSystem()
                {
                    Name = reader.GetString(0),
                    MinTurbinedOutflow = Convert.ToDouble(reader.GetValue(1)),
                    MaxTurbinedOutflow = Convert.ToDouble(reader.GetValue(2)),
                    EnergyFactor = Convert.ToDouble(reader.GetValue(3)),
                    StartPeriod = Convert.ToInt32(reader.GetValue(4)),
                    Id = Convert.ToInt32(reader.GetValue(5))
                });
            }
            DataBaseManager.DbConnection.Close();

            return hydroSystems;
        }

        public static int UpdateObject(HydroSystem dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT sistema " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(sistema, turbinamientoMinimo, turbinamientoMaximo, FactorEnergia, EtapaEntrada) " +
                        "VALUES(@Name, @MinTurbinedOutflow, @MaxTurbinedOutflow, @EnergyFactor, @InitialPeriod)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "sistema = @Name, " +
                                        "turbinamientoMinimo = @MinTurbinedOutflow, " +
                                        "turbinamientoMaximo = @MaxTurbinedOutflow, " +
                                        "FactorEnergia = @EnergyFactor, " +
                                        "EtapaEntrada = @InitialPeriod " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@MinTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@MaxTurbinedOutflow", OleDbType.Numeric);
                command.Parameters.Add("@EnergyFactor", OleDbType.Numeric);
                command.Parameters.Add("@InitialPeriod", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@MinTurbinedOutflow"].Value = dataObject.MinTurbinedOutflow;
                command.Parameters["@MaxTurbinedOutflow"].Value = dataObject.MaxTurbinedOutflow;
                command.Parameters["@EnergyFactor"].Value = dataObject.EnergyFactor;
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

        public static void DeleteObject(HydroSystem dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);

            query = string.Format("DELETE FROM topologiaHidraulica " +
                                  "WHERE Sistema = '{0}'", dataObject.Name);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


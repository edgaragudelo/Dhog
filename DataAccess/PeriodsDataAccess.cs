using DHOG_WPF.Models;
using DHOG_WPF.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;

namespace DHOG_WPF.DataAccess
{
    public class PeriodsDataAccess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PeriodsDataAccess));
        private static string table = "periodoBasica";

        public static List<Period> GetObjects()
        {
            List<Period> periods = new List<Period>();

            string query = string.Format("SELECT Fecha, nombre, demanda, duracionHoras, ReservaAGC, CostoRacionamiento, CAR, demandaInternacional, tasaDescuento, Escenario, Id " +
                                         "FROM {0} ORDER BY Escenario, Nombre", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                periods.Add(new Period()
                {
                        
                    Date = reader.GetString(0),
                    Name = Convert.ToInt32(reader.GetValue(1)),
                    Load = Convert.ToDouble(reader.GetValue(2)),
                    HourlyDuration = Convert.ToDouble(reader.GetValue(3)),
                    AGCReservoir = Convert.ToDouble(reader.GetValue(4)),
                    RationingCost = Convert.ToDouble(reader.GetValue(5)),
                    CAR = Convert.ToDouble(reader.GetValue(6)),
                    InternationalLoad = Convert.ToDouble(reader.GetValue(7)),
                    DiscountRate = Convert.ToDouble(reader.GetValue(8)),
                    Case = Convert.ToInt32(reader.GetValue(9)),
                    Id = Convert.ToInt32(reader.GetValue(10))
                });
            }
            DataBaseManager.DbConnection.Close();

            return periods;
        }

        public static int UpdateObject(Period dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Fecha, nombre, demanda, duracionHoras, ReservaAGC, CostoRacionamiento, CAR, demandaInternacional, tasaDescuento, Escenario) " +
                                        "VALUES(@Date, @Name, @Load, @HourlyDuration, @AGCReservoir, @RationingCost, @CAR, @InternationalLoad, @DiscountRate, @Case)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Fecha = @Date, " +
                                        "nombre = @Name, " +
                                        "demanda = @Load, " +
                                        "duracionHoras = @HourlyDuration, " +
                                        "ReservaAGC = @AGCReservoir, " +
                                        "CostoRacionamiento = @RationingCost, " +
                                        "CAR = @CAR, " +
                                        "demandaInternacional = @InternationalLoad, " +
                                        "tasaDescuento = @DiscountRate, " +
                                        "Escenario = @Case " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Date", OleDbType.VarChar);
                command.Parameters.Add("@Name", OleDbType.Numeric);
                command.Parameters.Add("@Load", OleDbType.Numeric);
                command.Parameters.Add("@HourlyDuration", OleDbType.Numeric);
                command.Parameters.Add("@AGCReservoir", OleDbType.Numeric);
                command.Parameters.Add("@RationingCost", OleDbType.Numeric);
                command.Parameters.Add("@CAR", OleDbType.Numeric);
                command.Parameters.Add("@InternationalLoad", OleDbType.Numeric);
                command.Parameters.Add("@DiscountRate", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Date"].Value = dataObject.Date;
                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Load"].Value = dataObject.Load;
                command.Parameters["@HourlyDuration"].Value = dataObject.HourlyDuration;
                command.Parameters["@AGCReservoir"].Value = dataObject.AGCReservoir;
                command.Parameters["@RationingCost"].Value = dataObject.RationingCost;
                command.Parameters["@CAR"].Value = dataObject.CAR;
                command.Parameters["@InternationalLoad"].Value = dataObject.InternationalLoad;
                command.Parameters["@DiscountRate"].Value = dataObject.DiscountRate;
                command.Parameters["@Case"].Value = dataObject.Case;
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

        public static void DeleteObject(Period dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }

        public static string[] GetPeriodsDate()
        {
            string[] periodsDate;

            string query = "SELECT MAX(Nombre) FROM periodoBasica WHERE Escenario = 1";
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (reader.Read())
            {
                object maxPeriod = reader.GetValue(0);
                if (!maxPeriod.ToString().Equals(""))
                    periodsDate = new string[Convert.ToInt32(reader.GetValue(0))];
                else
                    periodsDate = null;
            }
            else
                periodsDate = null;

            DataBaseManager.DbConnection.Close();
            
            query = "SELECT Nombre, Fecha FROM periodoBasica WHERE Escenario = 1";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                int periodPosition = Convert.ToInt32(reader.GetValue(0)) - 1;
                periodsDate[periodPosition] = reader.GetString(1);
            }
            DataBaseManager.DbConnection.Close();
            
            return periodsDate;
        }
    }
}

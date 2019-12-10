using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicBarraDataAccess
    {
        public static string table = "BarraPeriodo";

        public static List<PeriodicBarra> GetPeriodicBarra()
        {
            List<PeriodicBarra> periodicBarra = new List<PeriodicBarra>();

            string query = String.Format("SELECT Nombre, Periodo, demanda, maximoAngulo, costoracionamiento " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicBarra.Add(new PeriodicBarra(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4))));

            DataBaseManager.DbConnection.Close();
            
            return periodicBarra;
        }

        public static void UpdatePeriodicBarra(PeriodicBarra periodicBarra)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "demanda = @Demanda, " +
                                         "maximoAngulo = @MaximoAngulo " +
                                         "costoracionamiento = @Costoracionamiento " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Demanda", OleDbType.Numeric);
                command.Parameters.Add("@MaximoAngulo", OleDbType.Numeric);
                command.Parameters.Add("@Costoracionamiento", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Demanda"].Value = periodicBarra.Demanda;
                command.Parameters["@MaximoAngulo"].Value = periodicBarra.MaximoAngulo;
                command.Parameters["@Costoracionamiento"].Value = periodicBarra.Costoracionamiento;
                command.Parameters["@Name"].Value = periodicBarra.Name;
                command.Parameters["@Period"].Value = periodicBarra.Periodo;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicBarra(PeriodicBarra periodicBarra)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, periodicBarra.Name, periodicBarra.Periodo);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicAreasDataAccess
    {
        public static string table = "areaPeriodo";

        public static List<PeriodicArea> GetPeriodicAreas()
        {
            List<PeriodicArea> periodicAreas = new List<PeriodicArea>();

            string query = String.Format("SELECT Nombre, Periodo, Demanda, limiteImportacion, limiteExportacion " +
                           "FROM {0} " +
                           "ORDER BY Periodo, Nombre ASC", table);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicAreas.Add(new PeriodicArea(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4))));
            
            DataBaseManager.DbConnection.Close();

            return periodicAreas;
        }

        public static void UpdatePeriodicArea(PeriodicArea periodicArea)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Demanda = @Load, " +
                                         "limiteImportacion = @ImportationLimit, " +
                                         "limiteExportacion = @ExportationLimit " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Load", OleDbType.Numeric);
                command.Parameters.Add("@ImportationLimit", OleDbType.Numeric);
                command.Parameters.Add("@ExportationLimit", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Load"].Value = periodicArea.Load;
                command.Parameters["@ImportationLimit"].Value = periodicArea.ImportationLimit;
                command.Parameters["@ExportationLimit"].Value = periodicArea.ExportationLimit;
                command.Parameters["@Name"].Value = periodicArea.Name;
                command.Parameters["@Period"].Value = periodicArea.Period;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicArea(PeriodicArea periodicArea)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = '{1}' " +
                                         "AND Periodo = {2}",
                                         table, periodicArea.Name, periodicArea.Period);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


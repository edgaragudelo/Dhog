using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicCompaniesDataAccess
    {
        public static string table = "empresaPeriodo";

        public static List<PeriodicCompany> GetPeriodicCompanies()
        {
            List<PeriodicCompany> periodicCompanies = new List<PeriodicCompany>();

            string query = String.Format("SELECT Nombre, Periodo, PrecioBolsa, Contrato, Escenario " +
                           "FROM {0} " +
                           "ORDER BY Escenario, Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicCompanies.Add(new PeriodicCompany(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4))));

            DataBaseManager.DbConnection.Close();

            return periodicCompanies;
        }

        public static void UpdatePeriodicCompany(PeriodicCompany periodicCompany)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "PrecioBolsa = @StockPrice, " +
                                         "Contrato = @Contract " +
                                         "WHERE Nombre = @Name AND " +
                                         "periodo = @Period AND " +
                                         "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@StockPrice", OleDbType.Numeric);
                command.Parameters.Add("@Contract", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@StockPrice"].Value = periodicCompany.StockPrice;
                command.Parameters["@Contract"].Value = periodicCompany.Contract;
                command.Parameters["@Name"].Value = periodicCompany.Name;
                command.Parameters["@Period"].Value = periodicCompany.Period;
                command.Parameters["@Case"].Value = periodicCompany.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicCompany(PeriodicCompany periodicCompany)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Nombre = '{1}' " +
                                         "AND Periodo = {2} " +
                                         "AND Escenario = {3}",
                                         table, periodicCompany.Name, periodicCompany.Period, periodicCompany.Case);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


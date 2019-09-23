using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicBlocksDataAccess
    {
        public static string table = "BloquePeriodo";

        public static List<PeriodicBlock> GetPeriodicBlocks()
        {
            List<PeriodicBlock> periodicBlocks = new List<PeriodicBlock>();

            string query = String.Format("SELECT Nombre, Periodo, FactorDuracion, FactorDemanda " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicBlocks.Add(new PeriodicBlock(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return periodicBlocks;
        }

        public static void UpdatePeriodicBlock(PeriodicBlock periodicBlock)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "FactorDuracion = @DurationFactor, " +
                                         "FactorDemanda = @LoadFactor " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@DurationFactor", OleDbType.Numeric);
                command.Parameters.Add("@LoadFactor", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.Numeric);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@DurationFactor"].Value = periodicBlock.DurationFactor;
                command.Parameters["@LoadFactor"].Value = periodicBlock.LoadFactor;
                command.Parameters["@Name"].Value = periodicBlock.Block;
                command.Parameters["@Period"].Value = periodicBlock.Period;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicBlock(PeriodicBlock periodicBlock)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, periodicBlock.Block, periodicBlock.Period);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


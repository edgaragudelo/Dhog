using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class PeriodicLoadBlocksDataAccess
    {
        public static string table = "DemandaBloque";

        public static List<PeriodicLoadBlock> GetPeriodicLoadBlocks()
        {
            List<PeriodicLoadBlock> periodicBlocks = new List<PeriodicLoadBlock>();

            string query = String.Format("SELECT Bloque, Periodo, Demanda, Escenario " +
                                         "FROM {0} " +
                                         "ORDER BY Escenario, Bloque, Periodo ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                periodicBlocks.Add(new PeriodicLoadBlock(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();

            return periodicBlocks;
        }

        public static void UpdatePeriodicLoadBlock(PeriodicLoadBlock periodicBlock)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Demanda = @Load " +
                                         "WHERE bloque = @Block AND " +
                                         "periodo = @Period AND " +
                                         "escenario = @Case", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Load", OleDbType.Numeric);
                command.Parameters.Add("@Block", OleDbType.Numeric);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Load"].Value = periodicBlock.Load;
                command.Parameters["@Block"].Value = periodicBlock.Block;
                command.Parameters["@Period"].Value = periodicBlock.Period;
                command.Parameters["@Case"].Value = periodicBlock.Case;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeletePeriodicLoadBlock(PeriodicLoadBlock periodicBlock)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Bloque = {1} " +
                                         "AND Periodo = {2} " +
                                         "AND Escenario = {3}",
                                         table, periodicBlock.Block, periodicBlock.Period, periodicBlock.Case);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


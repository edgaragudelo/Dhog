using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class AreaBarraDataAccess
    {
        public static string table = "AreaPeriodo";

        public static List<AreaBarra> GetAreaBarra()
        {
            List<AreaBarra> AreaBarra = new List<AreaBarra>();

            string query = String.Format("SELECT Nombre, Periodo, demanda, limiteImportacion, limiteExportacion " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                AreaBarra.Add(new AreaBarra(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4))));

            DataBaseManager.DbConnection.Close();
            
            return AreaBarra;
        }

        public static void UpdateAreaBarra(AreaBarra AreaBarra)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "demanda = @Demanda, " +
                                         "limiteImportacion = @limiteImportacion " +
                                         "limiteExportacion = @limiteExportacion " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Demanda", OleDbType.Numeric);
                command.Parameters.Add("@limiteImportacion", OleDbType.Numeric);
                command.Parameters.Add("@limiteExportacion", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Demanda"].Value = AreaBarra.Demanda;
                command.Parameters["@limiteImportacion"].Value = AreaBarra.LimiteImportacion;
                command.Parameters["@limiteExportacion"].Value = AreaBarra.LimiteExportacion;
                command.Parameters["@Name"].Value = AreaBarra.Name;
                command.Parameters["@Period"].Value = AreaBarra.Periodo;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteAreaBarra(AreaBarra AreaBarra)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, AreaBarra.Name, AreaBarra.Periodo);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


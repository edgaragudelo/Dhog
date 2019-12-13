using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class CortePeriodoDataAccess
    {
        public static string table = "CortePeriodo";

        public static List<CortePeriodo> GetCortePeriodo()
        {
            List<CortePeriodo> CortePeriodo = new List<CortePeriodo>();

            string query = String.Format("SELECT Nombre, Periodo, importacion, exportacion " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                CortePeriodo.Add(new CortePeriodo(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return CortePeriodo;
        }

        public static void UpdateCortePeriodo(CortePeriodo CortePeriodo)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "importacion = @importacion, " +
                                         "exportacion = @exportacion, " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@importacion", OleDbType.Numeric);
                command.Parameters.Add("@exportacion", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@importacion"].Value = CortePeriodo.Importacion;
                command.Parameters["@exportacion"].Value = CortePeriodo.Exportacion;
                command.Parameters["@Name"].Value = CortePeriodo.Name;
                command.Parameters["@Period"].Value = CortePeriodo.Periodo;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteCortePeriodo(CortePeriodo CortePeriodo)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, CortePeriodo.Name, CortePeriodo.Periodo);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


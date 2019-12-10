using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class LineaPeriodoDataAccess
    {
        public static string table = "LineaPeriodo";

        public static List<LineaPeriodo> GetLineaPeriodo()
        {
            List<LineaPeriodo> LineaPeriodo = new List<LineaPeriodo>();

            string query = String.Format("SELECT Nombre, Periodo, Flujomaximo " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                LineaPeriodo.Add(new LineaPeriodo(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2))));

            DataBaseManager.DbConnection.Close();
            
            return LineaPeriodo;
        }

        public static void UpdateLineaPeriodo(LineaPeriodo LineaPeriodo)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Flujomaximo = @Flujomaximo, " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Flujomaximo", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Flujomaximo"].Value = LineaPeriodo.Flujomaximo;
                command.Parameters["@Name"].Value = LineaPeriodo.Name;
                command.Parameters["@Period"].Value = LineaPeriodo.Periodo;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteLineaPeriodo(LineaPeriodo LineaPeriodo)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, LineaPeriodo.Name, LineaPeriodo.Periodo);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


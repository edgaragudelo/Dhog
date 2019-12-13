using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class CorteLineaDataAccess
    {
        public static string table = "CorteLinea";

        public static List<CorteLinea> GetCorteLinea()
        {
            List<CorteLinea> CorteLinea = new List<CorteLinea>();

            string query = String.Format("SELECT Nombre, Linea, Sentido " +
                                         "FROM {0} " +
                                         "ORDER BY Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                CorteLinea.Add(new CorteLinea(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(1)), Convert.ToInt16(reader.GetValue(2))));

            DataBaseManager.DbConnection.Close();
            
            return CorteLinea;
        }

        public static void UpdateCorteLinea(CorteLinea CorteLinea)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Linea = @Linea, " +
                                         "WHERE nombre = @Name AND " +
                                         "Sentido = @Sentido", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Linea", OleDbType.VarChar);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Sentido", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Linea"].Value = CorteLinea.Linea;
                command.Parameters["@Name"].Value = CorteLinea.Name;
                command.Parameters["@Sentido"].Value = CorteLinea.Sentido;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteCorteLinea(CorteLinea CorteLinea)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Sentido = {2}",
                                         table, CorteLinea.Name, CorteLinea.Sentido);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


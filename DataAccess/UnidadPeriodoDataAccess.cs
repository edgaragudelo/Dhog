using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class UnidadPeriodoDataAccess
    {
        public static string table = "UnidadPeriodo";

        public static List<UnidadPeriodo> GetUnidadPeriodo()
        {
            List<UnidadPeriodo> UnidadPeriodo = new List<UnidadPeriodo>();

            string query = String.Format("SELECT Nombre, Periodo, Minimo, Maximo " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                UnidadPeriodo.Add(new UnidadPeriodo(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return UnidadPeriodo;
        }

        public static void UpdateUnidadPeriodo(UnidadPeriodo UnidadPeriodo)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Minimo = @Minimo, " +
                                         "Maximo = @Maximo, " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Minimo", OleDbType.Numeric);
                command.Parameters.Add("@Maximo", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Minimo"].Value = UnidadPeriodo.Minimo;
                command.Parameters["@Maximo"].Value = UnidadPeriodo.Maximo;
                command.Parameters["@Name"].Value = UnidadPeriodo.Name;
                command.Parameters["@Period"].Value = UnidadPeriodo.Periodo;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteUnidadPeriodo(UnidadPeriodo UnidadPeriodo)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, UnidadPeriodo.Name, UnidadPeriodo.Periodo);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


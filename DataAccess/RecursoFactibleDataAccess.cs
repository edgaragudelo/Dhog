using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class RecursoFactibleDataAccess
    {
        public static string table = "RecursoFactible";

        public static List<RecursoFactible> GetRecursoFactible()
        {
            List<RecursoFactible> RecursoFactible = new List<RecursoFactible>();

            string query = String.Format("SELECT Nombre, indice, Minimo, Maximo " +
                                         "FROM {0} " +
                                         "ORDER BY indice, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                RecursoFactible.Add(new RecursoFactible(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)),Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return RecursoFactible;
        }

        public static void UpdateRecursoFactible(RecursoFactible RecursoFactible)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Minimo = @Minimo, " +
                                         "Maximo = @Maximo, " +
                                         "WHERE nombre = @Name AND " +
                                         "indice = @indice", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Minimo", OleDbType.Numeric);
                command.Parameters.Add("@Maximo", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@indice", OleDbType.Numeric);
               


                DataBaseManager.DbConnection.Open();

                command.Parameters["@Minimo"].Value = RecursoFactible.Minimo;
                command.Parameters["@Maximo"].Value = RecursoFactible.Maximo;
                command.Parameters["@Name"].Value = RecursoFactible.Name;
                command.Parameters["@indice"].Value = RecursoFactible.indice;
               

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteRecursoFactible(RecursoFactible RecursoFactible)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND indice = {2}",
                                         table, RecursoFactible.Name, RecursoFactible.indice);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


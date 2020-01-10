using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class RecursoUnidadDataAccess
    {
        public static string table = "RecursoUnidad";

        public static List<RecursoUnidad> GetRecursoUnidad()
        {
            List<RecursoUnidad> RecursoUnidad = new List<RecursoUnidad>();

            string query = String.Format("SELECT Nombre, Unidad " +
                                         "FROM {0} " +
                                         "ORDER BY Unidad ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                RecursoUnidad.Add(new RecursoUnidad(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(1))));

            DataBaseManager.DbConnection.Close();
            
            return RecursoUnidad;
        }

        public static void UpdateRecursoUnidad(RecursoUnidad RecursoUnidad)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Unidad = @Unidad, " +
                                         "WHERE nombre = @Name " , table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Unidad", OleDbType.Numeric);              
                command.Parameters.Add("@Name", OleDbType.VarChar);
                

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Unidad"].Value = RecursoUnidad.Unidad;                
                command.Parameters["@Name"].Value = RecursoUnidad.Name;
               

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteRecursoUnidad(RecursoUnidad RecursoUnidad)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Unidad = {2}",
                                         table, RecursoUnidad.Name, RecursoUnidad.Unidad);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


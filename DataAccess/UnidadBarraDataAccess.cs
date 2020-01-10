using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class UnidadBarraDataAccess
    {
        public static string table = "UnidadBarra";

        public static List<UnidadBarra> GetUnidadBarra()
        {
            List<UnidadBarra> UnidadBarra = new List<UnidadBarra>();

            string query = String.Format("SELECT Nombre, Barra " +
                                         "FROM {0} " +
                                         "ORDER BY Barra ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                UnidadBarra.Add(new UnidadBarra(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(1))));

            DataBaseManager.DbConnection.Close();
            
            return UnidadBarra;
        }

        public static void UpdateUnidadBarra(UnidadBarra UnidadBarra)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Barra = @Barra, " +
                                         "WHERE nombre = @Name " , table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Barra", OleDbType.Numeric);              
                command.Parameters.Add("@Name", OleDbType.VarChar);
                

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Barra"].Value = UnidadBarra.Barra;                
                command.Parameters["@Name"].Value = UnidadBarra.Name;
               

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteUnidadBarra(UnidadBarra UnidadBarra)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Barra = {2}",
                                         table, UnidadBarra.Name, UnidadBarra.Barra);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


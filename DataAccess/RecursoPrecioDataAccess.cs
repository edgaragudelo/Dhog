using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class RecursoPrecioDataAccess
    {
        public static string table = "RecursoPrecio";

        public static List<RecursoPrecio> GetRecursoPrecio()
        {
            List<RecursoPrecio> RecursoPrecio = new List<RecursoPrecio>();

            string query = String.Format("SELECT Nombre, Precio " +
                                         "FROM {0} " +
                                         "ORDER BY Precio ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                RecursoPrecio.Add(new RecursoPrecio(Convert.ToString(reader.GetValue(0)), Convert.ToDouble(reader.GetValue(1))));

            DataBaseManager.DbConnection.Close();
            
            return RecursoPrecio;
        }

        public static void UpdateRecursoPrecio(RecursoPrecio RecursoPrecio)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Precio = @Precio, " +
                                         "WHERE nombre = @Name " , table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Precio", OleDbType.Numeric);              
                command.Parameters.Add("@Name", OleDbType.VarChar);
                

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Precio"].Value = RecursoPrecio.Precio;                
                command.Parameters["@Name"].Value = RecursoPrecio.Name;
               

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteRecursoPrecio(RecursoPrecio RecursoPrecio)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Precio = {2}",
                                         table, RecursoPrecio.Name, RecursoPrecio.Precio);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


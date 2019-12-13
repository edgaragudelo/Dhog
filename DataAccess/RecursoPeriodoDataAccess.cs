using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class RecursoPeriodoDataAccess
    {
        public static string table = "RecursoPeriodo";

        public static List<RecursoPeriodo> GetRecursoPeriodo()
        {
            List<RecursoPeriodo> RecursoPeriodo = new List<RecursoPeriodo>();

            string query = String.Format("SELECT Nombre, Periodo, Minimo, Maximo, Precio,Obligatorio,Pruebas,AGC " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                RecursoPeriodo.Add(new RecursoPeriodo(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)),Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4)), 
                    Convert.ToDouble(reader.GetValue(5)), Convert.ToString(reader.GetValue(6)), Convert.ToDouble(reader.GetValue(7))));

            DataBaseManager.DbConnection.Close();
            
            return RecursoPeriodo;
        }

        public static void UpdateRecursoPeriodo(RecursoPeriodo RecursoPeriodo)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Minimo = @Minimo, " +
                                         "Maximo = @Maximo, " +
                                         "Precio = @Precio, " +
                                         "Obligatorio = @Obligatorio, " +
                                         "Pruebas = @Pruebas, " +
                                         "AGC = @AGC, " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Minimo", OleDbType.Numeric);
                command.Parameters.Add("@Maximo", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);
                command.Parameters.Add("@Precio", OleDbType.Numeric);
                command.Parameters.Add("@Obligatorio", OleDbType.Numeric);                
                command.Parameters.Add("@Pruebas", OleDbType.VarChar);
                command.Parameters.Add("@AGC", OleDbType.Numeric);


                DataBaseManager.DbConnection.Open();

                command.Parameters["@Minimo"].Value = RecursoPeriodo.Minimo;
                command.Parameters["@Maximo"].Value = RecursoPeriodo.Maximo;
                command.Parameters["@Name"].Value = RecursoPeriodo.Name;
                command.Parameters["@Period"].Value = RecursoPeriodo.Periodo;
                command.Parameters["@Precio"].Value = RecursoPeriodo.Precio;
                command.Parameters["@Obligatorio"].Value = RecursoPeriodo.Obligatorio;
                command.Parameters["@Pruebas"].Value = RecursoPeriodo.Pruebas;
                command.Parameters["@AGC"].Value = RecursoPeriodo.AGC;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteRecursoPeriodo(RecursoPeriodo RecursoPeriodo)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, RecursoPeriodo.Name, RecursoPeriodo.Periodo);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


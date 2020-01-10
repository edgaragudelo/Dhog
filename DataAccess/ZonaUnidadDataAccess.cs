using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class ZonaUnidadDataAccess
    {
        public static string table = "ZonaUnidad";

        public static List<ZonaUnidad> GetZonaUnidad()
        {
            List<ZonaUnidad> ZonaUnidad = new List<ZonaUnidad>();

            string query = String.Format("SELECT Nombre, Unidad, Periodo,  Peso " +
                                         "FROM {0} " +
                                         "ORDER BY Periodo, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                ZonaUnidad.Add(new ZonaUnidad(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return ZonaUnidad;
        }

        public static void UpdateZonaUnidad(ZonaUnidad ZonaUnidad)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Unidad = @Unidad, " +
                                         "Peso = @Peso, " +
                                         "WHERE nombre = @Name AND " +
                                         "periodo = @Period", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Unidad", OleDbType.Numeric);
                command.Parameters.Add("@Peso", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Period", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Unidad"].Value = ZonaUnidad.Unidad;
                command.Parameters["@Peso"].Value = ZonaUnidad.Peso;
                command.Parameters["@Name"].Value = ZonaUnidad.Name;
                command.Parameters["@Period"].Value = ZonaUnidad.Periodo;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteZonaUnidad(ZonaUnidad ZonaUnidad)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Periodo = {2}",
                                         table, ZonaUnidad.Name, ZonaUnidad.Periodo);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


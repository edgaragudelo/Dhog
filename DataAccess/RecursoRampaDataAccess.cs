using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class RecursoRampaDataAccess
    {
        public static string table = "RecursoRampa";

        public static List<RecursoRampa> GetRecursoRampa()
        {
            List<RecursoRampa> RecursoRampa = new List<RecursoRampa>();

            string query = String.Format("SELECT Nombre, Configuracion, Tipo, Indice, Modelo,Valor " +
                                         "FROM {0} " +
                                         "ORDER BY Configuracion, Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                RecursoRampa.Add(new RecursoRampa(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)),Convert.ToDouble(reader.GetValue(3)), Convert.ToString(reader.GetValue(4)), 
                    Convert.ToDouble(reader.GetValue(5))));

            DataBaseManager.DbConnection.Close();
            
            return RecursoRampa;
        }

        public static void UpdateRecursoRampa(RecursoRampa RecursoRampa)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "Tipo = @Tipo, " +
                                         "Indice = @Indice, " +
                                         "Modelo = @Modelo, " +
                                         "Valor = @Valor, " +                                         
                                         "WHERE nombre = @Name AND " +
                                         "Configuracion = @Configuracion", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Tipo", OleDbType.Numeric);
                command.Parameters.Add("@Indice", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Configuracion", OleDbType.Numeric);
                command.Parameters.Add("@Modelo", OleDbType.Numeric);
                command.Parameters.Add("@Valor", OleDbType.Numeric);                
              


                DataBaseManager.DbConnection.Open();

                command.Parameters["@Tipo"].Value = RecursoRampa.Tipo;
                command.Parameters["@Indice"].Value = RecursoRampa.Indice;
                command.Parameters["@Name"].Value = RecursoRampa.Name;
                command.Parameters["@Configuracion"].Value = RecursoRampa.Configuracion;
                command.Parameters["@Modelo"].Value = RecursoRampa.Modelo;
                command.Parameters["@Valor"].Value = RecursoRampa.Valor;
               

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteRecursoRampa(RecursoRampa RecursoRampa)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " +
                                         "AND Configuracion = {2}",
                                         table, RecursoRampa.Name, RecursoRampa.Configuracion);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


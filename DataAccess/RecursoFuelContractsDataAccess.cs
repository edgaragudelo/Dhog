using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;

namespace DHOG_WPF.DataAccess
{
    public class RecursoFuelContractsDataAccess
    {
        private static string table = "ContratoCombustibleRecurso";

        public static List<RecursoFuelContract> GetObjects()
        {
            List<RecursoFuelContract> plants = new List<RecursoFuelContract>();

            string query = string.Format("SELECT Nombre, Recurso, Id " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new RecursoFuelContract()
                {
                    Name1 = reader.GetString(0),
                    //Recurso = reader.GetString(1),
                    Name = reader.GetString(1),
                    Id = Convert.ToInt32(reader.GetValue(2))
                });
            }
            DataBaseManager.DbConnection.Close();

            return plants;
        }

        public static int UpdateObject(RecursoFuelContract dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT Nombre,Recurso " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Nombre, Recurso) " +
                                        "VALUES(@Name, @Recurso)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Nombre = @Name1, " +
                                        "Recurso = @Recurso " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name1", OleDbType.VarChar);
                command.Parameters.Add("@Recurso", OleDbType.VarChar);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name1"].Value = dataObject.Name1;
                command.Parameters["@Recurso"].Value = dataObject.Name;//    dataObject.Recurso;
                command.Parameters["@Id"].Value = dataObject.Id;


                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    DataBaseManager.DbConnection.Close();
                    MessageBox.Show("Error", ex.Message);
                    throw;
                }
                DataBaseManager.DbConnection.Close();
            }

            if (isNew)
            {
                int id;
                query = string.Format("SELECT Max(Id) FROM {0}", table);
                reader = DataBaseManager.ReadData(query);
                reader.Read();
                id = Convert.ToInt32(reader.GetValue(0));
                DataBaseManager.DbConnection.Close();
                return id;
            }
            else
                return -1;
        }

        public static void DeleteObject(RecursoFuelContract dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class PFEquationsDataAccess
    {
        private static string table = "ecuacionesFC";

        public static List<PFEquation> GetObjects()
        {
            List<PFEquation> pfEquations = new List<PFEquation>();

            string query = string.Format("SELECT Recurso, Embalse, Intercepto, coeficienteLineal, coeficienteCuadratico, Escenario, Id " +
                                         "FROM {0} ORDER BY Recurso, Embalse, Escenario", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                pfEquations.Add(new PFEquation()
                {
                    Name = reader.GetString(0),
                    Reservoir = reader.GetString(1),
                    Intercept = Convert.ToDouble(reader.GetValue(2)),
                    LinearCoefficient = Convert.ToDouble(reader.GetValue(3)),
                    CuadraticCoefficient = Convert.ToDouble(reader.GetValue(4)),
                    Case = Convert.ToInt32(reader.GetValue(5)),
                    Id = Convert.ToInt32(reader.GetValue(6))
                });
            }
            DataBaseManager.DbConnection.Close();
            
            return pfEquations;
        }

        public static int UpdateObject(PFEquation dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT Recurso " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Recurso, Embalse, Intercepto, coeficienteLineal, coeficienteCuadratico, Escenario) " +
                                      "VALUES(@Name, @Reservoir, @Intercept, @LinearCoefficient, @CuadraticCoefficient, @Case)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "Recurso = @Name, " +
                                        "Embalse = @Reservoir, " +
                                        "Intercepto = @Intercept, " +
                                        "coeficienteLineal = @LinearCoefficient, " +
                                        "coeficienteCuadratico = @CuadraticCoefficient, " +
                                        "escenario = @Case " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Reservoir", OleDbType.VarChar);
                command.Parameters.Add("@Intercept", OleDbType.Numeric);
                command.Parameters.Add("@LinearCoefficient", OleDbType.Numeric);
                command.Parameters.Add("@CuadraticCoefficient", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Reservoir"].Value = dataObject.Reservoir;
                command.Parameters["@Intercept"].Value = dataObject.Intercept;
                command.Parameters["@LinearCoefficient"].Value = dataObject.LinearCoefficient;
                command.Parameters["@CuadraticCoefficient"].Value = dataObject.CuadraticCoefficient;
                command.Parameters["@Case"].Value = dataObject.Case;
                command.Parameters["@Id"].Value = dataObject.Id;

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                }
                catch
                {
                    DataBaseManager.DbConnection.Close();
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
        

        public static void DeleteObject(PFEquation dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


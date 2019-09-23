using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class CompaniesDataAccess
    {
        private static string table = "empresaBasica";

        public static List<Company> GetObjects()
        {
            List<Company> companies = new List<Company>();

            string query = string.Format("SELECT Nombre, PrecioBolsa, Contrato, ModelaContratos, FactorContrato, FactorPenalizacionContrato, Escenario, Id " +
                                         "FROM {0} ORDER BY ModelaContratos DESC, Escenario, Nombre", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                companies.Add(new Company()
                {
                    Name = reader.GetString(0),
                    StockPrice = Convert.ToDouble(reader.GetValue(1)),
                    Contract = Convert.ToDouble(reader.GetValue(2)),
                    IsContractModeled = Convert.ToInt32(reader.GetValue(3)),
                    ContractFactor = Convert.ToDouble(reader.GetValue(4)),
                    ContractPenalizationFactor = Convert.ToDouble(reader.GetValue(5)),
                    Case = Convert.ToInt32(reader.GetValue(6)),
                    Id = Convert.ToInt32(reader.GetValue(7))
                });
            }
            DataBaseManager.DbConnection.Close();
            return companies;
        }

        public static int UpdateObject(Company dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(nombre, PrecioBolsa, Contrato, ModelaContratos, FactorContrato, FactorPenalizacionContrato, Escenario) " +
                        "VALUES(@Name, @StockPrice, @Contract, @IsContractModeled, @ContractFactor, @ContractPenalizationFactor, @Case)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "PrecioBolsa = @StockPrice, " +
                                        "Contrato = @Contract, " +
                                        "ModelaContratos = @IsContractModeled, " +
                                        "FactorContrato = @ContractFactor, " +
                                        "FactorPenalizacionContrato = @ContractPenalizationFactor," +
                                        "escenario = @Case " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@StockPrice", OleDbType.Numeric);
                command.Parameters.Add("@Contract", OleDbType.Numeric);
                command.Parameters.Add("@IsContractModeled", OleDbType.Numeric);
                command.Parameters.Add("@ContractFactor", OleDbType.Numeric);
                command.Parameters.Add("@ContractPenalizationFactor", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@StockPrice"].Value = dataObject.StockPrice;
                command.Parameters["@Contract"].Value = dataObject.Contract;
                command.Parameters["@IsContractModeled"].Value = dataObject.IsContractModeled;
                command.Parameters["@ContractFactor"].Value = dataObject.ContractFactor;
                command.Parameters["@ContractPenalizationFactor"].Value = dataObject.ContractPenalizationFactor;
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

        public static void DeleteObject(Company dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


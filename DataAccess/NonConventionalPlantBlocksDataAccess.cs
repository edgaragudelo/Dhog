using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class NonConventionalPlantBlocksDataAccess
    {
        private static string table = "recursoNoCoBloque";

        public static List<NonConventionalPlantBlock> GetObjects()
        {
            List<NonConventionalPlantBlock> blocks = new List<NonConventionalPlantBlock>();

            //string query = string.Format("SELECT nombre, Bloque, FactorReductor, Id " +
            //                             "FROM {0} " +
            //                             "ORDER BY nombre, Bloque", table);

            string query = string.Format("SELECT tipo, periodo, bloque, factorReductor,Id " +
                                  "FROM {0} " +
                                  "ORDER BY tipo,periodo, Bloque", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                try
                {
                    blocks.Add(new NonConventionalPlantBlock()
                {
                    Name = reader.GetString(0),
                    Case = Convert.ToInt32(reader.GetValue(1)),
                    Block = Convert.ToInt32(reader.GetValue(2)),
                    ReductionFactor = Convert.ToDouble(reader.GetValue(3)),
                    Id = Convert.ToInt32(reader.GetValue(4))
                });
                }
                catch (Exception e)
                {

                    throw;
                }

               
            }
            DataBaseManager.DbConnection.Close();
            return blocks;
        }

        public static int UpdateObject(NonConventionalPlantBlock dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT tipo " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                //query = string.Format("INSERT INTO {0} (tipo, periodo, Bloque, FactorReductor,id) " +
                //                        "VALUES(@Name, @Case,@Block, @ReductionFactor, @Id)", table);
                query = string.Format("INSERT INTO {0} (tipo, periodo, Bloque, FactorReductor) " +
                                     "VALUES(@Name, @Case,@Block, @ReductionFactor)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "tipo = @Name, " +
                                        "Bloque = @Block, " +
                                        "Periodo = @Case," +
                                        "FactorReductor = @ReductionFactor " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@Block", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@ReductionFactor", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@Block"].Value = dataObject.Block;
                command.Parameters["@Case"].Value = dataObject.Case;
                command.Parameters["@ReductionFactor"].Value = dataObject.ReductionFactor;
                command.Parameters["@Id"].Value = dataObject.Id;

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception e)
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

        public static void DeleteObject(NonConventionalPlantBlock dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

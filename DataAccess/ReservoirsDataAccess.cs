using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class ReservoirsDataAccess
    {
        private static string table = "embalseBasica";

        public static List<Reservoir> GetObjects()
        {
            List<Reservoir> plants = new List<Reservoir>();
            string formato = string.Format((Char)(34) + "##.00" + (Char)(34));
            string query = string.Format("SELECT Nombre, VolMinimo, VolMaximo, VolumenInicial, VolumenFinal, Filtracion, FactorRecuperacion, FactorPenalizacionVertimiento, empresa, EtapaEntrada, Escenario, Id " +
                                         "FROM {0}", table);
            //query = query.Replace("VolMinimo", "Format(VolMinimo," + formato + ") as VolMinimo1");
            //query = query.Replace("VolMaximo", "Format(VolMaximo," + formato + ") as VolMaximo1");
            //query = query.Replace("VolumenInicial", "Format(VolumenInicial," + formato + ") as VolumenInicial1");
            //query = query.Replace("VolumenFinal", "Format(VolumenFinal," + formato + ") as VolumenFinal1");

            //query = query.Replace("\\"," ");
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                plants.Add(new Reservoir()
                {
                    Name = reader.GetString(0),
                    MinLevel = Convert.ToDouble(reader.GetValue(1)),
                    MaxLevel = Convert.ToDouble(reader.GetValue(2)),
                    InitialLevel = Convert.ToDouble(reader.GetValue(3)),
                    FinalLevel = Convert.ToDouble(reader.GetValue(4)),
                    Filtration = Convert.ToDouble(reader.GetValue(5)),
                    RecoveryFactor = Convert.ToDouble(reader.GetValue(6)),
                    SpillagePenalizationFactor = Convert.ToDouble(reader.GetValue(7)),
                    Company = reader.GetString(8),
                    StartPeriod = Convert.ToInt32(reader.GetValue(9)),
                    Case = Convert.ToInt32(reader.GetValue(10)),
                    Id = Convert.ToInt32(reader.GetValue(11))
                });
            }
            DataBaseManager.DbConnection.Close();

            return plants;
        }

        public static int UpdateObject(Reservoir dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE Id = {1}", table, dataObject.Id);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(Nombre, VolMinimo, VolMaximo, VolumenInicial, VolumenFinal, Filtracion, FactorRecuperacion, FactorPenalizacionVertimiento, empresa, EtapaEntrada, Escenario) " +
                                        "VALUES(@Name, @MinLevel, @MaxLevel, @InitialLevel, @FinalLevel, @Filtration, @RecoveryFactor, @SpillagePenalizationFactor, @Company, @StartPeriod, @Case)", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "VolMinimo = @MinLevel, " +
                                        "VolMaximo = @MaxLevel, " +
                                        "VolumenInicial = @InitialLevel, " +
                                        "VolumenFinal = @FinalLevel, " +
                                        "Filtracion = @Filtration, " +
                                        "FactorRecuperacion = @RecoveryFactor, " +
                                        "FactorPenalizacionVertimiento = @SpillagePenalizationFactor, " +
                                        "empresa = @Company, " +
                                        "EtapaEntrada = @StartPeriod, " +
                                        "escenario = @Case " +
                                        "WHERE Id = @Id", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@MinLevel", OleDbType.Numeric);
                command.Parameters.Add("@MaxLevel", OleDbType.Numeric);
                command.Parameters.Add("@InitialLevel", OleDbType.Numeric);
                command.Parameters.Add("@FinalLevel", OleDbType.Numeric);
                command.Parameters.Add("@Filtration", OleDbType.Numeric);
                command.Parameters.Add("@RecoveryFactor", OleDbType.Numeric);
                command.Parameters.Add("@SpillagePenalizationFactor", OleDbType.Numeric);
                command.Parameters.Add("@Company", OleDbType.VarChar);
                command.Parameters.Add("@StartPeriod", OleDbType.Numeric);
                command.Parameters.Add("@Case", OleDbType.Numeric);
                command.Parameters.Add("@Id", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Name;
                command.Parameters["@MinLevel"].Value = dataObject.MinLevel;
                command.Parameters["@MaxLevel"].Value = dataObject.MaxLevel;
                command.Parameters["@InitialLevel"].Value = dataObject.InitialLevel;
                command.Parameters["@FinalLevel"].Value = dataObject.FinalLevel;
                command.Parameters["@Filtration"].Value = dataObject.Filtration;
                command.Parameters["@RecoveryFactor"].Value = dataObject.RecoveryFactor;
                command.Parameters["@SpillagePenalizationFactor"].Value = dataObject.SpillagePenalizationFactor; 
                command.Parameters["@Company"].Value = dataObject.Company;
                command.Parameters["@StartPeriod"].Value = dataObject.StartPeriod;
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

        public static void DeleteObject(Reservoir dataObject)
        {
            string query = string.Format("DELETE FROM {0} " + 
                                         "WHERE Id = {1}", table, dataObject.Id);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

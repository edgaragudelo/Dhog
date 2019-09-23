using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class ScenariosDataAccess
    {
        private static string table = "escenariosBasica";

        public static List<Scenario> GetScenarios()
        {
            List<Scenario> scenarios = new List<Scenario>();

           string query = string.Format("SELECT Variable, numeroEscenarios, activo, etapaArbol " +
                                         "FROM {0}", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                scenarios.Add(new Scenario(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3))));
                           
            
            DataBaseManager.DbConnection.Close();
            
            return scenarios;
        }


        public static List<ScenariosActivos> GetScenariosActivos()
        {
            List<ScenariosActivos> scenariosactivos = new List<ScenariosActivos>();
         
            string query = string.Format(" select distinct 'Escenarios',Escenario,Escenario,Escenario " +
                                        "FROM zz_DespachoRecurso");

            OleDbDataReader reader = DataBaseManager.ReadDataOut(query);

            try
            {


                while (reader.Read())
                       scenariosactivos.Add(new ScenariosActivos(reader.GetString(0), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3))));
                    //NumeroScenarios = Convert.ToInt32(reader.GetValue(0));
                //foreach (ScenariosActivos item in scenariosactivos)
                //    {
                //        item.CasesQuantity = Convert.ToInt32(reader.GetValue(1));
                //    }
            }
            catch (Exception)
            {
                throw;
            }


            DataBaseManager.OutputDbConnection.Close();

            return scenariosactivos;
        }


        public static void UpdateScenario(Scenario scenario)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "numeroEscenarios = @CasesQuantity, " +
                                         "activo = @IsActive, " +
                                         "etapaArbol = @TreePeriod " +
                                         "WHERE Variable = @Variable", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@CasesQuantity", OleDbType.Numeric);
                command.Parameters.Add("@IsActive", OleDbType.Numeric);
                command.Parameters.Add("@TreePeriod", OleDbType.VarChar);
                command.Parameters.Add("@Variable", OleDbType.VarChar);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@CasesQuantity"].Value = scenario.CasesQuantity;
                command.Parameters["@IsActive"].Value = scenario.IsActive;
                command.Parameters["@TreePeriod"].Value = scenario.TreePeriod;
                command.Parameters["@Variable"].Value = scenario.Variable;

                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }
    }
}


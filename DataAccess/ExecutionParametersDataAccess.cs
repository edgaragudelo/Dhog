using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class ExecutionParameterssDataAccess
    {
        
        public static ExecutionParameters GetExecutionParameters()
        {
            ExecutionParameters executionParameters = new ExecutionParameters();

            string query = string.Format("SELECT valor " +
                                         "FROM configuracionProblema " +
                                         "WHERE PrioridadUI = 0 " +
                                         "ORDER BY nombre");
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (reader.Read())
                executionParameters.Case = Convert.ToInt32(reader.GetValue(0));

            if (reader.Read())
                executionParameters.IsIterative = Convert.ToInt32(reader.GetValue(0));

            if (reader.Read())
                executionParameters.ObjectiveFunction = Convert.ToInt32(reader.GetValue(0));
            
            DataBaseManager.DbConnection.Close();

            query = string.Format("SELECT EtapaInicial, EtapaFinal " +
                                  "FROM horizonte ");
            reader = DataBaseManager.ReadData(query);
            if (reader.Read())
            {
                executionParameters.InitialPeriod = Convert.ToInt32(reader.GetValue(0));
                executionParameters.FinalPeriod = Convert.ToInt32(reader.GetValue(1));
            }
            DataBaseManager.DbConnection.Close();

            return executionParameters;
        }

        public static void UpdateInitialPeriod(int value)
        {
            string query = string.Format("UPDATE horizonte SET EtapaInicial = {0}", value);
            DataBaseManager.ExecuteQuery(query);
        }

        public static void UpdateFinalPeriod(int value)
        {
            string query = string.Format("UPDATE horizonte SET EtapaFinal = {0}", value);
            DataBaseManager.ExecuteQuery(query);
        }

        public static void UpdateCase(int value)
        {
            string query = string.Format("UPDATE configuracionProblema " +
                                         "SET valor = {0} " +
                                         "WHERE nombre = 'ESCENARIO'", value);
            DataBaseManager.ExecuteQuery(query);
        }

        public static void UpdateIsIterative(int value)
        {
            string query = string.Format("UPDATE configuracionProblema " +
                                         "SET valor = {0} " +
                                         "WHERE nombre = 'ITERATIVO'", value);
            DataBaseManager.ExecuteQuery(query);
        }

        public static void UpdateObjectiveFuncion(int value)
        {
            string query = string.Format("UPDATE configuracionProblema " +
                                         "SET valor = {0} " +
                                         "WHERE nombre = 'MODELO'", value);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

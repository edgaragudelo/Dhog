﻿using log4net;
using System;
using System.Data.OleDb;
using System.Windows;

namespace DHOG_WPF.DataAccess
{
    class DataBaseManager
    {
        public static OleDbConnection DbConnection { get; private set; }
        public static OleDbConnection OutputDbConnection { get; private set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(DataBaseManager));
       

        public DataBaseManager(string inputDataSource, string outputDataSource, string tipoBD)
        {
          
            if (tipoBD=="Access") 
            {
                DbConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + inputDataSource);
                OutputDbConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + outputDataSource);             
            }

            if (tipoBD == "Sql Server") 
            {
                DbConnection = new OleDbConnection(inputDataSource);
               OutputDbConnection = new OleDbConnection(outputDataSource);
            }

        }

        public static OleDbDataReader ReadData(string query)
        {
            OleDbDataReader reader;
            using (OleDbCommand command = new OleDbCommand(query, DbConnection))
            {
                DbConnection.Open();
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception e)
                {
                    log.Fatal(e.Message);
                    DbConnection.Close();
                    throw;
                }
            }
            return reader;
        }

        public static OleDbDataReader ReadDataOut(string query)
        {
            OleDbDataReader reader1;
            using (OleDbCommand command = new OleDbCommand(query, OutputDbConnection))
            {
                OutputDbConnection.Open();
                try
                {
                    reader1 = command.ExecuteReader();
                }
                catch (Exception e)
                {
                    log.Fatal(e.Message);
                    OutputDbConnection.Close();
                    throw;
                }
            }
            return reader1;
        }


        public static void ExecuteQuery_Output(string query)
        {
            using (OleDbCommand command = new OleDbCommand(query, OutputDbConnection))
            {
                OutputDbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    OutputDbConnection.Close();
                }
                catch (Exception e)
                {
                    log.Fatal(e.Message);
                    OutputDbConnection.Close();
                    throw;
                }
            }
        }



        public static void ExecuteQuery(string query)
        {
            using (OleDbCommand command = new OleDbCommand(query, DbConnection))
            {
                DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    DbConnection.Close();
                }
                catch (Exception e)
                {
                    log.Fatal(e.Message);
                    DbConnection.Close();
                    throw;
                }
            }
        }
    }
}

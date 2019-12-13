using System;
using System.Collections.Generic;
using System.Data.OleDb;
using DHOG_WPF.Models;


namespace DHOG_WPF.DataAccess
{
    public class RecursoBasicaDataAccess
    {
        public static string table = "RecursoBasica";

        public static List<RecursoBasica> GetRecursoBasica()
        {
            List<RecursoBasica> RecursoBasica = new List<RecursoBasica>();

            string query = String.Format("SELECT nombre, TML, TMFL, genIni, tlIni, tflIni, configuracion, maximoarranques, modelaRampas, modelaintervalos, costoArranque " +
                                         "FROM {0} " +
                                         "ORDER BY Nombre ASC", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                RecursoBasica.Add(new RecursoBasica(Convert.ToString(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)),Convert.ToDouble(reader.GetValue(3)), Convert.ToDouble(reader.GetValue(4)), 
                    Convert.ToDouble(reader.GetValue(5)), Convert.ToDouble(reader.GetValue(6)), Convert.ToDouble(reader.GetValue(7)),Convert.ToDouble(reader.GetValue(8)),Convert.ToDouble(reader.GetValue(9)),Convert.ToDouble(reader.GetValue(10))));

            DataBaseManager.DbConnection.Close();
            
            return RecursoBasica;
        }

        public static void UpdateRecursoBasica(RecursoBasica RecursoBasica)
        {
            string query = string.Format("UPDATE {0} SET " +
                                         "TML = @TML, " +
                                         "TMFL = @TMFL, " +
                                         "genIni = @genIni, " +
                                         "tlIni = @tlIni, " +
                                         "tflIni = @tflIni, " +
                                         "configuracion = @configuracion, " +
                                         "maximoarranques = @maximoarranques, " +
                                         "modelaRampas = @modelaRampas, " +
                                         "modelaintervalos = @modelaintervalos, " +
                                         "costoArranque = @costoArranque, " +
                                         "WHERE nombre = @Name ", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@TML", OleDbType.Numeric);
                command.Parameters.Add("@TMFL", OleDbType.Numeric);
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@genIni", OleDbType.Numeric);
                command.Parameters.Add("@tlIni", OleDbType.Numeric);                
                command.Parameters.Add("@tflIni", OleDbType.Numeric);
                command.Parameters.Add("@configuracion", OleDbType.Numeric);
                command.Parameters.Add("@maximoarranques", OleDbType.Numeric);
                command.Parameters.Add("@modelaRampas", OleDbType.Numeric);
                command.Parameters.Add("@modelaintervalos", OleDbType.Numeric);
                command.Parameters.Add("@costoArranque", OleDbType.Numeric);


                DataBaseManager.DbConnection.Open();

                command.Parameters["@TML"].Value = RecursoBasica.TML;
                command.Parameters["@TMFL"].Value = RecursoBasica.TMFL;
                command.Parameters["@Name"].Value = RecursoBasica.Name;                
                command.Parameters["@genIni"].Value = RecursoBasica.GenIni;
                command.Parameters["@tlIni"].Value = RecursoBasica.TlIni;
                command.Parameters["@tflIni"].Value = RecursoBasica.TflIni;
                command.Parameters["@configuracion"].Value = RecursoBasica.Configuracion;
                command.Parameters["@maximoarranques"].Value = RecursoBasica.Maximoarranques;
                command.Parameters["@modelaRampas"].Value = RecursoBasica.ModelaRampas;
                command.Parameters["@modelaintervalos"].Value = RecursoBasica.Modelaintervalos;
                command.Parameters["@costoArranque"].Value = RecursoBasica.CostoArranque;


                int rowsAffected = command.ExecuteNonQuery();

                DataBaseManager.DbConnection.Close();
            }
        }

        public static void DeleteRecursoBasica(RecursoBasica RecursoBasica)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE nombre = {1} " ,table,RecursoBasica.Name);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}


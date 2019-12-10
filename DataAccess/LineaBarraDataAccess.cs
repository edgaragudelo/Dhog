using DHOG_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    public class LineaBarraDataAccess
    {
        private static string table = "LineaBarra";

        public static List<LineaBarra> GetObjects()
        {
            List<LineaBarra> LineaBarra = new List<LineaBarra>();

            string query = string.Format("SELECT nombre, barraInicial, barraFinal, reactancia, nMenos1, FlujoMaximo, activa " +
                                         "FROM {0} ORDER BY nombre", table);
            OleDbDataReader reader = DataBaseManager.ReadData(query);
            
            while (reader.Read())
            {

                LineaBarra.Add(new LineaBarra(Convert.ToString(reader.GetValue(0)),Convert.ToString(reader.GetValue(1)),Convert.ToString(reader.GetValue(2)),
                    Convert.ToDouble(reader.GetValue(3)),Convert.ToDouble(reader.GetValue(4)),Convert.ToInt32(reader.GetValue(5)),Convert.ToInt32(reader.GetValue(6))));

            }
            DataBaseManager.DbConnection.Close();

            return LineaBarra;
        }

        public static int UpdateObject(LineaBarra dataObject)
        {
            bool isNew = false;
            string query = string.Format("SELECT nombre " +
                                         "FROM {0} " +
                                         "WHERE nombre = {1}", table, dataObject.Nombre);

            OleDbDataReader reader = DataBaseManager.ReadData(query);
            if (!reader.Read())
            {
                query = string.Format("INSERT INTO {0}(nombre, barraInicial, barraFinal, reactancia, nMenos1, FlujoMaximo, activa) " +
                                        "VALUES(@Nombre, @BarraInicial, @BarraFinal, @Reactancia,@NMenos1,@FlujoMaximo,@Activa )", table);
                isNew = true;
            }
            else
            {
                query = string.Format("UPDATE {0} SET " +
                                        "nombre = @Name, " +
                                        "BarraInicial = @BarraInicial" +
                                        "BarraFinal = @BarraFinal" +
                                        "Reactancia = @Reactancia, " +
                                        "NMenos1 = @NMenos1, " +
                                        "FlujoMaximo = @FlujoMaximo " +
                                        "WHERE Activa = @Activa", table);
            }
            DataBaseManager.DbConnection.Close();

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@Name", OleDbType.VarChar);
                command.Parameters.Add("@BarraInicial", OleDbType.VarChar);
                command.Parameters.Add("@BarraFinal", OleDbType.VarChar);
                command.Parameters.Add("@Reactancia", OleDbType.Numeric);
                command.Parameters.Add("@NMenos1", OleDbType.Numeric);
                command.Parameters.Add("@FlujoMaximo", OleDbType.Numeric);
                command.Parameters.Add("@Activa", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                command.Parameters["@Name"].Value = dataObject.Nombre;
                command.Parameters["@BarraInicial"].Value = dataObject.BarraInicial;
                command.Parameters["@Reactancia"].Value = dataObject.Reactancia;
                command.Parameters["@NMenos1"].Value = dataObject.NMenos1;
                command.Parameters["@FlujoMaximo"].Value = dataObject.FlujoMaximo;
                command.Parameters["@Activa"].Value = dataObject.Activa;

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
                //query = string.Format("SELECT Max(Id) FROM {0}", table);
                //reader = DataBaseManager.ReadData(query);
                //reader.Read();
                //int id = Convert.ToInt32(reader.GetValue(0));
                //DataBaseManager.DbConnection.Close();
                //return id;
                return 1;
            }
            else
                return -1;
        }

        public static void DeleteObject(LineaBarra dataObject)
        {
            string query = string.Format("DELETE FROM {0} " +
                                         "WHERE Nombre = {1}", table, dataObject.Nombre);
            DataBaseManager.ExecuteQuery(query);
        }
    }
}

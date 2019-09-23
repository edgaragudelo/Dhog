using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.DataAccess
{
    public class ResultsReader
    {
        public static DataTable ReadMarginalAverage()
        {
            string table = null;
            string query = null;
            table = "zz_escenarios";
            DataTable dataTable = new DataTable();
            query = string.Format("SELECT escenario from {0}", table);
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            int escenario1;
            DataTable dataTable1 = new DataTable();
            foreach (DataRow row in dataTable.Rows)
            {
                
                escenario1 = Convert.ToInt32(row[0]);
                table = "zz_marginalPromedio";
                

                query = string.Format("(SELECT top 1 escenario, 'Porc95' AS vble, porc95 AS marginal FROM(SELECT TOP 95 PERCENT escenario, zz_marginalPromedio.marginal AS porc95 " +
                                            "FROM zz_marginalPromedio where escenario=" + escenario1 + " ORDER BY zz_marginalPromedio.marginal ASC) " +
                                            "group by escenario, porc95 order by porc95 desc) UNION " +
                                            "(SELECT top 1 escenario, 'Porc5' AS vble, porc5 AS marginal FROM(SELECT TOP 5 PERCENT escenario, zz_marginalPromedio.marginal AS porc5 " +
                                            "FROM zz_marginalPromedio where escenario=" + escenario1 + " ORDER BY zz_marginalPromedio.marginal ASC) " + 
                                            " group by  escenario, porc5 order by porc5 desc) UNION " +
                                            "SELECT escenario, 'Minimo' AS vble, MIN(marginal) AS minimo  FROM zz_marginalPromedio where escenario=" + escenario1 + 
                                            " GROUP BY escenario UNION " +
                                            "SELECT escenario, 'Maximo' AS vble, MAX(marginal) AS maximo  FROM zz_marginalPromedio where escenario=" + escenario1 + 
                                            " GROUP BY escenario", table, escenario1);

                OleDbDataAdapter adapter1 = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);

                try
                {
                    adapter1.Fill(dataTable1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return dataTable1;
        }

        public static DataTable LLenarDatosObjetivo(int scenario, string selctedCompany)
        {
            string table = "Empresabasica, periodobasica";
            DataTable dataTable = new DataTable();

            string query = string.Format("SELECT Empresabasica.nombre, periodobasica.escenario, periodobasica.nombre, contrato as generacion " +
                            "FROM {0} " +
                            "WHERE Empresabasica.escenario = {1} AND Empresabasica.nombre = '{2}' and periodobasica.escenario = Empresabasica.escenario ORDER BY PERIODOBASICA.NOMBRE",
                            table, scenario, selctedCompany);        


            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.DbConnection);

            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadGenerationByCompany(int scenario, string selctedCompany)
        {
            string table = "zz_generacionEmpresa";
            DataTable dataTable = new DataTable();

            string query = string.Format("SELECT empresa, escenario, periodo, generacion " +
                            "FROM {0} " +
                            "WHERE escenario = {1} AND empresa = '{2}' ORDER BY PERIODO, ESCENARIO",
                            table, scenario, selctedCompany);

            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);

            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadObjectiveGenerationByCompany(int scenario, string selctedCompany)
        {
            string table = "empresaPeriodo";
            DataTable dataTable = new DataTable();

            string query = string.Format("SELECT nombre as empresa, escenario, periodo, contrato AS generacion " +
                            "FROM {0} " +
                            "WHERE escenario = {1} AND nombre = '{2}' ORDER BY PERIODO, ESCENARIO;",
                            table, scenario, selctedCompany);

            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.DbConnection);

            try
            {
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count ==0)
                {
                    dataTable=LLenarDatosObjetivo(scenario,selctedCompany);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadSINGeneration(int scenario, SINGenerationType generationType)
        {
            string table = null;
            switch (generationType)
            {
                case SINGenerationType.Daily:
                    table = "zz_ReporteGeneracion_dia";
                    break;
                case SINGenerationType.Hourly:
                    table = "zz_ReporteGeneracion_hora";
                    break;
                case SINGenerationType.Period:
                    table = "zz_ReporteGeneracion";
                    break;
            }

            DataTable dataTable = new DataTable();
            string formato = string.Format((Char)(34) + "##.00" + (Char)(34));
            string query = string.Format("SELECT Periodo, racionamiento, GMenor, GEolica, GGeotermia, GSolar, GBiomasa, GCarbon, GGas, GFuel, GHidro, Demanda " +
                            "FROM {0} " +
                            "WHERE Escenario = {1}", table, scenario);


            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadDailyThermalGeneration(int scenario)
        {
            string table = "zz_ReporteGeneracion_dia";
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT Periodo, GCarbon, GGas, GFuel " +
                            "FROM {0} " +
                            "WHERE Escenario = {1}", table, scenario);
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadSINReservoirsEvolution(int scenario)
        {
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT p.periodo, p.volumen as [% Volumen en HM3], e.volumen as [% Volumen en GWh] " +
                                         "FROM zz_EvolucionEmbalseSIN as p " +
                                         "INNER JOIN zz_EvolucionEmbalseSIN_E as e " +
                                         "ON p.periodo = e.periodo AND p.escenario = e.escenario " +
                                         "WHERE p.escenario = {0}", scenario);
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadReservoirEvolution(int scenario, string selectedReservoir)
        {
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT v.periodo, v.volumen as [Volumen], v.volmin as [NEP], d.despacho as [Generación] " +
                                         "FROM zz_volumenUtil as v, zz_despachoRecurso_P as d, MapeoEmbalses as m " +
                                         "WHERE v.periodo = d.periodo " +
                                         "AND v.escenario = d.escenario " +
                                         "AND v.embalse = m.embalse " +
                                         "AND m.recurso = d.recurso " +
                                         "AND v.embalse = '{0}' " +
                                         "AND v.escenario = {1}  order by v.periodo" , selectedReservoir, scenario);
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadMarginalCostsByBlocks(int scenario, int tipo)
        {
            DataTable dataTable = new DataTable();
            string query = null;
            if (tipo == 0)
            {
                query = string.Format("TRANSFORM Max(zz_costoMarginal.CostoMarginal) " +
                                             "SELECT zz_costoMarginal.periodo " +
                                             "FROM zz_costoMarginal " +
                                             "WHERE zz_costoMarginal.escenario = {0} " +
                                             "GROUP BY zz_costoMarginal.periodo " +
                                             "PIVOT zz_costoMarginal.Bloque", scenario);
            }
            else
            {
                query = "TRANSFORM Max(zz_costoMarginal.CostoMarginal) " +
                                           "SELECT zz_costoMarginal.periodo " +
                                           "FROM zz_costoMarginal " +
                                           "GROUP BY zz_costoMarginal.periodo " +
                                           "PIVOT zz_costoMarginal.Escenario";

            }
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable ReadMarginalCost(int scenario)
        {
            string table = "zz_marginalPromedio";
            if (scenario == 0)
                scenario = 1;
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT Periodo, marginal " +
                            "FROM {0} " +
                            "WHERE Escenario = {1}", table, scenario);
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return dataTable;
        }

        public static DataTable GetResultsFromTable(string table)
        {
            DataTable dataTable = new DataTable();
            string query = null;
            if (table != "ESCENARIO_OUT")
            {
                query = string.Format("SELECT b.fecha,a.* FROM {0} ", table);
                query = query + " a,periodobasica b where a.periodo=b.nombre";
            }
            else
                query = string.Format("SELECT * FROM {0} ", table);

            OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.OutputDbConnection);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error de Datos", e.Message);
            }

            return dataTable;
        }
    }
}

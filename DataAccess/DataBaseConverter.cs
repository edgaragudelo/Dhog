using DHOG_WPF.Models;
using DHOG_WPF.Util;
using DHOG_WPF.ViewModels;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;


namespace DHOG_WPF.DataAccess
{
    class DataBaseConverter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DataBaseConverter));
        public int Warnings { get; private set; }

        public DataBaseConverter()
        {
            Warnings = 0;
        }

        public void ConvertDBToCurrentVersion(DHOGDataBaseViewModel dhogDB)
        {
            GlobalContext.Properties["DBConversionLogFileName"] = dhogDB.DBFolder + "\\DHOG_ConversionBD.log";
            XmlConfigurator.Configure();
            log.Info(MessageUtil.FormatMessage("INFO.DBVersion", dhogDB.Version));

            while (dhogDB.Version < DHOGMainWindow.DHOGVersion)
            {
                if (dhogDB.Version < 3.2)
                {
                    CreateDBInfoTable();
                    CreatePlantsMappingTables();
                    CreatePeriodicHydroElementTable();
                    CreateIdsForBasicTables();
                    CreateSubareaColumnInPlantsTables();
                    CreateFuelColumnInThermalPlantTable();
                    CreateElementTypeColumnInHydroTopologyTable();
                    AlterProblemConfigurationTable();
                    UpdateCplexParameterDescription();
                    ChangeColumnsType();
                    AddPrimaryKeyToPeriodsTable();
                    AddPrimaryKeyToScenariosTable();
                    AddPrimaryKeyToHydroSystemTable();
                    AddPrimaryKeyToVariableThermalPlantTable();
                    AddPrimaryKeyToHydroTopologyTable();
                    AddPrimaryKeyToPFEquationsTable();
                    AddPrimaryKeyToExcludingPlantsTable();
                    ReplaceNullsWithZero();
                    CreateDBZonaEspecial();
                    CreateDBRutasDhog();
                    dhogDB.Version = 3.2;
                    log.Info(MessageUtil.FormatMessage("INFO.DBVersion", dhogDB.Version));
                }
            }
        }

        public void CreateDBInfoTable()
        {
            string table = "InfoBD";
            string query = string.Format("CREATE TABLE {0} (Id Int NOT NULL, Descripcion Text, VersionDHOG Double, PRIMARY KEY (ID))", table);
            DataBaseManager.ExecuteQuery(query);

            query = string.Format("INSERT INTO {0}(Id, Descripcion, VersionDHOG) " +
                                  "VALUES (1, 'Descripción autogenerada por el sistema', 3.2)", table);
            DataBaseManager.ExecuteQuery(query);

            log.Info(MessageUtil.FormatMessage("INFO.TableCreated", table));
        }

        public void CreateDBZonaEspecial()
        {
            string table = "zonaEspecial";
            string query = string.Format("CREATE TABLE {0} (Nombre LONGCHAR, IndiceIni SMALLINT, IndiceFin SMALLINT,Id autoincrement, PRIMARY KEY (ID))", table);
            DataBaseManager.ExecuteQuery(query);

            query = string.Format("INSERT INTO {0}(Nombre, IndiceIni, IndiceFin,Id) " +
                                  "VALUES ('Sogamoso',1,12,1)", table);
            DataBaseManager.ExecuteQuery(query);

            log.Info(MessageUtil.FormatMessage("INFO.TableCreated", table));
        }

        public void CreateDBRutasDhog()
        {
            string table = "RutasDhog";
            string query = string.Format("CREATE TABLE {0} (Id autoincrement,  RutaModelo LONGCHAR, RutaEjecutable LONGCHAR, RutaBD LONGCHAR, RutaSalida LONGCHAR, RutaSolver LONGCHAR, PRIMARY KEY (ID))", table);
            DataBaseManager.ExecuteQuery(query);

            query = string.Format("INSERT INTO {0} (ID, RutaModelo, RutaEjecutable, RutaBD, RutaSalida, RutaSolver)" +
                                  "VALUES (1,'Modelo','Ejecutable','BD','Salida','Solver')", table);
            DataBaseManager.ExecuteQuery(query);

            log.Info(MessageUtil.FormatMessage("INFO.TableCreated", table));
        }


     



        public void CreatePlantsMappingTables()
        {
            List<string> tables = new List<string>(){
                "MapeoRecursosHidro",
                "MapeoRecursosTermicos"
            };

            foreach (string table in tables)
            {
                string query = string.Format("SELECT TOP 1 Planta FROM {0}", table);
                try
                {
                    OleDbDataReader reader = DataBaseManager.ReadData(query);
                }
                catch
                {
                    query = string.Format("CREATE TABLE {0} (Planta Text, Recurso Text)", table);
                    DataBaseManager.ExecuteQuery(query);
                    log.Info(MessageUtil.FormatMessage("INFO.TableCreated", table));
                }

                DataBaseManager.DbConnection.Close();
            }
        }

        public void CreatePeriodicHydroElementTable()
        {
            bool alterTable = false;
            string table = "elementoHidraulicoPeriodo";
            string query = string.Format("SELECT * FROM {0}", table);
            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    query = string.Format("ALTER TABLE {0} DROP CONSTRAINT PrimaryKey", table);
                    alterTable = true;
                }
                catch
                {
                    query = string.Format("CREATE TABLE {0} (Nombre Text NOT NULL, Periodo Integer NOT NULL, TurMinimo Double, TurMaximo Double, Filtracion Double, FactorRecuperacion Double, PRIMARY KEY (Nombre, Periodo))", table);
                }
                DataBaseManager.DbConnection.Close();
            }

            DataBaseManager.ExecuteQuery(query);

            if (alterTable)
            {
                query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (Nombre, Periodo)", table);
                DataBaseManager.ExecuteQuery(query);
                log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
            }
            else
                log.Info(MessageUtil.FormatMessage("INFO.TableCreated", table));
        }

        public void CreateIdsForBasicTables()
        {
            List<string> tables = new List<string>
            {
                "areaBasica",
                "bloqueBasica",
                "combustibleBasica",
                "ContratoCombustibleBasica",
                "ContratoCombustibleRecurso",
                "elementoHidraulicoBasica",
                "embalseBasica",
                "empresaBasica",
                "periodoBasica",
                "recursoHidroBasica",
                "recursoNoCoBasica",
                "recursoTermicoBasica",
                "sistemaHidroBasica",
                "ZonaBasica",
                "recursoNoCoBloque",
                "recursoHidroVariable",
                "recursoTermicoVariable",
                "ecuacionesFC",
                "topologiaHidraulica",
                "recursosExcluyentes"
            };

            foreach (string table in tables)
            {
                string query = string.Format("ALTER TABLE {0} ADD Id AUTOINCREMENT", table);
                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    DataBaseManager.DbConnection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //TODO: Move to DBManager after testing
                    }
                    DataBaseManager.DbConnection.Close();
                }
            }

            log.Info(MessageUtil.FormatMessage("INFO.IdsCreatedInBasicTables"));
        }

        public void ReplaceNullsWithZero()
        {
            DataBaseManager.DbConnection.Open();
            List<string> tableNames = new List<string>();
            using (DataTable dbTables = DataBaseManager.DbConnection.GetSchema("TABLES"))
            {
                for (int i = 0; i < dbTables.Rows.Count; i++)
                    tableNames.Add(dbTables.Rows[i][2].ToString());
            }
            DataBaseManager.DbConnection.Close();

            foreach (string tableName in tableNames)
            {
                if (!tableName.Contains("MSys") && !tableName.Equals("Paste Errors") && !tableName.Contains("TMP"))
                {
                    if(tableName.Equals("zonaRecurso"))
                        Console.WriteLine("Table: " + tableName);

                    DataTable table = new DataTable();
                    string query = "SELECT * FROM " + tableName;
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, DataBaseManager.DbConnection);
                    try
                    {
                        adapter.Fill(table);
                    }
                    catch(Exception e)
                    {
                        log.Error(e.Message);
                    }

                    foreach (DataColumn column in table.Columns)
                    {
                        if (!column.ColumnName.Equals("Id"))
                        {
                            query = "UPDATE " + tableName + " " +
                                    "SET " + column.ColumnName + " = 0 " +
                                    "WHERE " + column.ColumnName + " IS NULL";

                            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                            {
                                DataBaseManager.DbConnection.Open();
                                try
                                {
                                    int affectedRows = command.ExecuteNonQuery();
                                    if (affectedRows > 0)
                                    {
                                        log.Warn(MessageUtil.FormatMessage("WARN.NullsFoundInTable", tableName, column.ColumnName));
                                        Warnings++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    DataBaseManager.DbConnection.Close();
                                    log.Error(e.Message);
                                }

                                DataBaseManager.DbConnection.Close();
                            }
                        }
                    }
                }
            }
        }

        public void CreateSubareaColumnInPlantsTables()
        {
            string column = "Subarea";
            List<string> tables = new List<string>
            {
                "recursoHidroBasica",
                "recursoNoCoBasica",
                "recursoTermicoBasica",
            };

            string query;
            foreach (string table in tables)
            {
                query = string.Format("ALTER TABLE {0} ADD {1} Text", table, column);
                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    DataBaseManager.DbConnection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //TODO: Move to DBManager after testing
                    }
                    DataBaseManager.DbConnection.Close();
                }

                query = string.Format("UPDATE {0} as r "+
                                      "INNER JOIN areaRecurso AS a " +
                                      "ON r.nombre = a.recurso " +
                                      "SET r.Subarea = a.nombre", table);
                DataBaseManager.ExecuteQuery(query);

                log.Info(MessageUtil.FormatMessage("INFO.ColumnCreatedInTable", column, table));
            }
            /*
            query = "DROP TABLE areaRecurso";
            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                command.ExecuteNonQuery();
                DataBaseManager.DbConnection.Close();
            }
            */
        }
        
        public void AlterProblemConfigurationTable()
        {
            string table = "configuracionProblema";
            string column = "PrioridadUI";

            string query = string.Format("ALTER TABLE {0} ADD {1} Int", table, column);
            DataBaseManager.ExecuteQuery(query);
            log.Info(MessageUtil.FormatMessage("INFO.ColumnCreatedInTable", column, table));

            column = "NombreUI";
            query = string.Format("ALTER TABLE {0} ADD {1} Text", table, column);
            DataBaseManager.ExecuteQuery(query);
            log.Info(MessageUtil.FormatMessage("INFO.ColumnCreatedInTable", column, table));

            List<ConfigurationParameter> variables = new List<ConfigurationParameter>()
            {
                new ConfigurationParameter("AREAS", 1, "Areas Operativas"),
                new ConfigurationParameter("CAR", 1, "Curva CAR"),
                new ConfigurationParameter("VOLUMENFINAL", 1, "Volumen Final"),
                new ConfigurationParameter("VERTIMIENTOS", 1, "Control de Vertimientos"),
                new ConfigurationParameter("PENALIZACIONV", 1, "Penalización de Vertimientos"),
                new ConfigurationParameter("ZONAS", 1, "Zonas de Seguridad"),
                new ConfigurationParameter("DEMANDA", 1, "Demanda"),
                new ConfigurationParameter("FCVARIABLE", 1, "Factor de Conversión Variable"),
                new ConfigurationParameter("FTVARIABLE", 1, "Factor de Consumo Variable"),
                new ConfigurationParameter("CONTRATOSCOMBUSTIBLE", 1, "Contratos Combustible"),
                new ConfigurationParameter("UTILVARIABLE", 1, "Volumen Útil Variable"),
                new ConfigurationParameter("DH", 1, "Energía Hidraúlica"),
                new ConfigurationParameter("GAS", 1, "Energía Térmica"),
                new ConfigurationParameter("CONTRATOSBILATERALES", 1, "Generación Mínima por Empresa"),
                new ConfigurationParameter("PERFILPERIODICO", 2, "Perfiles Periódicos"),
                new ConfigurationParameter("WARMSTART", 2, "Warmstart"),
                new ConfigurationParameter("TASADESCUENTO", 2, "Tasa de Descuento"),
                new ConfigurationParameter("COSTO1", 2, "Costos Variables"),
                new ConfigurationParameter("COSTO2", 2, "Costos de Combustible"),
                new ConfigurationParameter("COSTO3", 2, "Costos de Contratos de Combustible"),
                new ConfigurationParameter("GENERARLP", 2, "Generar Archivo LP"),
                new ConfigurationParameter("RGON", 2, "Modelar Unidades"),                
            };

            foreach (ConfigurationParameter variable in variables)
            {
                query = string.Format("UPDATE {0} " +
                                      "SET PrioridadUI = {1}, nombreUI = '{2}' " +
                                      "WHERE nombre = '{3}'", table, variable.Priority, variable.UIName, variable.Name);
                DataBaseManager.ExecuteQuery(query);
            }
        }

        public void UpdateCplexParameterDescription()
        {
            
            string query = string.Format("UPDATE parametrosCplex " +
                                         "SET Descripcion = 'Number of CPU cores that CPLEX actually uses during a parallel optimization' " +
                                         "WHERE nombre = 'threads'");
            DataBaseManager.ExecuteQuery(query);
            log.Info(MessageUtil.FormatMessage("INFO.CplexParameterDescriptionUpdated"));
        }

        public void CreateFuelColumnInThermalPlantTable()
        {
            string table = "recursoTermicoBasica";
            string column = "Combustible";

            string query = string.Format("ALTER TABLE {0} ADD {1} Text", table, column);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            query = string.Format("UPDATE {0} as r " +
                                    "INNER JOIN combustibleRecurso AS c " +
                                    "ON r.nombre = c.recurso " +
                                    "SET r.Combustible = c.nombre", table);
            DataBaseManager.ExecuteQuery(query);

            log.Info(MessageUtil.FormatMessage("INFO.ColumnCreatedInTable", column, table));
        }

        public void CreateElementTypeColumnInHydroTopologyTable()
        {
            string table = "topologiaHidraulica";
            string column = "TipoElemento";

            string query = string.Format("ALTER TABLE {0} ADD {1} Text", table, column);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            List<NameMapping> typeConversionMapping = new List<NameMapping>()
            {
                new NameMapping("'E', 'V', 'AV', 'A', 'AA'", "Embalse"),
                new NameMapping("'R'", "Rio"),
                new NameMapping("'TG'", "RecursoHidro"),
                new NameMapping("'T'", "ElementoHidro")
            };

            foreach (NameMapping mapping in typeConversionMapping) {
                query = string.Format("UPDATE {0} " +
                                      "SET TipoElemento = '{1}' " +
                                      "WHERE Tipo IN({2})", table, mapping.SDDPName, mapping.DHOGName);
                DataBaseManager.ExecuteQuery(query);
            }

            query = string.Format("UPDATE {0} " +
                                  "SET TipoElemento = 'RecursoHidro' " +
                                  "WHERE Tipo = 'AT' " +
                                  "AND topologiaHidraulica.Elemento IN (SELECT nombre " +
                                                                        "FROM RecursoHidroBasica " +
                                                                        "WHERE Escenario = 1)", table);
            DataBaseManager.ExecuteQuery(query);

            query = string.Format("UPDATE {0} " +
                                  "SET TipoElemento = 'ElementoHidro' " +
                                  "WHERE Tipo = 'AT' " +
                                  "AND topologiaHidraulica.Elemento IN (SELECT nombre " +
                                                                        "FROM elementoHidraulicoBasica)", table);
            DataBaseManager.ExecuteQuery(query);

            log.Info(MessageUtil.FormatMessage("INFO.ColumnCreatedInTable", column, table));
        }

        public void AddPrimaryKeyToPeriodsTable()
        {
            string table = "periodoBasica";

            string query = string.Format("ALTER TABLE {0} DROP CONSTRAINT PRIMARYKEY", table);
            DataBaseManager.ExecuteQuery(query);

            query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (nombre, escenario)", table);
            DataBaseManager.ExecuteQuery(query);
            /*
            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }
            */
            log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
        }

        public void AddPrimaryKeyToScenariosTable()
        {
            string table = "escenariosBasica";

            string query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (Variable)", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
        }

        public void AddPrimaryKeyToHydroSystemTable()
        {
            string table = "sistemaHidroBasica";

            string query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (sistema)", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
        }

        public void AddPrimaryKeyToVariableThermalPlantTable()
        {
            string table = "recursoTermicoVariable";

            string query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (Recurso, Segmento)", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
        }

        public void AddPrimaryKeyToHydroTopologyTable()
        {
            string table = "topologiaHidraulica";

            string query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (Id)", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
        }

        public void AddPrimaryKeyToPFEquationsTable()
        {
            string table = "ecuacionesFC";

            string query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (recurso, Embalse, escenario)", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
        }

        public void AddPrimaryKeyToExcludingPlantsTable()
        {
            string table = "recursosExcluyentes";

            string query = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (Id)", table);

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                DataBaseManager.DbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO: Move to DBManager after testing
                }
                DataBaseManager.DbConnection.Close();
            }

            log.Info(MessageUtil.FormatMessage("INFO.PrimaryKeyAddedToTable", table));
        }

        public void ChangeColumnsType()
        {
            Dictionary<string, string> tableColumnDictionary = new Dictionary<string, string>
            {
                { "combustibleBasica", "MinimoHora"},
                { "combustiblePeriodo", "MinimoHora"},
                { "recursoHidroBasica", "Maximo"},
                { "recursoHidroPeriodo", "Maximo"}
            };
            string dataType = "Double";

            string query;
            foreach (KeyValuePair<string, string> entry in tableColumnDictionary)
            {
                query = string.Format("ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {2}", entry.Key, entry.Value, dataType);
                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    DataBaseManager.DbConnection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //TODO: Move to DBManager after testing
                    }
                    DataBaseManager.DbConnection.Close();
                    log.Info(MessageUtil.FormatMessage("INFO.ColumnTypeChangedInTable", entry.Value, dataType, entry.Key));
                }
            }
        }
    }

    class ConfigurationParameter
    {
        public ConfigurationParameter(string name, int priority, string uIName)
        {
            Name = name;
            Priority = priority;
            UIName = uIName;
        }

        public string Name { get; }
        public int Priority { get; }
        public string UIName { get; }
    }
}

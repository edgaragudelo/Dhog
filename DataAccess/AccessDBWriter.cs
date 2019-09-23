using DHOG_WPF.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DHOG_WPF.DataAccess
{
    class AccessDBWriter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccessDBWriter));
        private OleDbDataReader reader;
        private string query;
        private FilesReadingParameters filesReadingParameters;

        public AccessDBWriter(FilesReadingParameters filesReadingParameters)
        {
            this.filesReadingParameters = filesReadingParameters;
        }

        public void DeleteDataFromTable(string table)
        {
            query = "DELETE FROM " + table;
            DataBaseManager.ExecuteQuery(query);
        }

        public void DeleteScenarioFromTable(string table, int scenario)
        {
            query = "DELETE FROM " + table + " WHERE Escenario = " + scenario;
            DataBaseManager.ExecuteQuery(query);
        }

        public void DeleteOtherScenariosFromTable(string table, int scenario)
        {
            query = "DELETE FROM " + table + " WHERE Escenario <> " + scenario;
            DataBaseManager.ExecuteQuery(query);
        }

        public int GetNextScenario(string table)
        {
            int scenario;
            query = "SELECT MAX(Escenario) FROM " + table;
            reader = DataBaseManager.ReadData(query);
            reader.Read();
            if (reader.GetValue(0) != DBNull.Value)
                scenario = Convert.ToInt32(reader.GetValue(0)) + 1;
            else
                scenario = 1;

            DataBaseManager.DbConnection.Close();
            
            return scenario;
        }

        public void WriteLoadBlocks(List<Block> loadBlocks)
        {
            string table = "BloqueBasica";
            DeleteDataFromTable(table);

            query = "INSERT INTO " + table + "(nombre, FactorDuracion, FactorDemanda) " +
                    "VALUES(@block, @durationFactor, @loadFactor)";
            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@block", OleDbType.Numeric);
                command.Parameters.Add("@durationFactor", OleDbType.Numeric);
                command.Parameters.Add("@loadFactor", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                foreach (Block loadBlock in loadBlocks)
                {
                    command.Parameters["@block"].Value = loadBlock.Name;
                    command.Parameters["@durationFactor"].Value = loadBlock.DurationFactor;
                    command.Parameters["@loadFactor"].Value = loadBlock.LoadFactor;
                    command.ExecuteNonQuery();
                }

                DataBaseManager.DbConnection.Close();
            }
        }

        public int GetScenario(bool createScenario, bool automaticScenarioCreation, int scenarioToCreate, string table, bool isPeriodic)
        {
            int scenario;
            if (createScenario && automaticScenarioCreation)
                scenario = GetNextScenario(table);
            else if (createScenario && scenarioToCreate == -1)
                scenario = 1;
            else if (createScenario && scenarioToCreate != -1)
                scenario = scenarioToCreate;
            else
            {
                scenario = 1;
                DeleteOtherScenariosFromTable(table, 1);
            }
            
            if(isPeriodic)
                DeleteScenarioFromTable(table, scenario);

            return scenario;
        }

        public void WritePeriodicLoad(double[,] load, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "DemandaBloque";
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, true);
            
            query = "INSERT INTO " + table + "(Periodo, Bloque, Demanda, Escenario) " +
                    "VALUES(@period, @block, @load, @scenario)";
            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@period", OleDbType.Numeric);
                command.Parameters.Add("@block", OleDbType.Numeric);
                command.Parameters.Add("@load", OleDbType.Numeric);
                command.Parameters.Add("@scenario", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                for(int period = 0; period < load.GetLength(0); period++)
                {
                    for (int block = 0; block < load.GetLength(1); block++)
                    {
                        command.Parameters["@period"].Value = period + filesReadingParameters.InitialPeriodDB;
                        command.Parameters["@block"].Value = block + 1;
                        command.Parameters["@load"].Value = load[period, block];
                        command.Parameters["@scenario"].Value = scenario;
                        command.ExecuteNonQuery();
                    }
                }

                DataBaseManager.DbConnection.Close();
            }
        }

        public void WritePeriodicRiversInflows(List<River> rivers, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "aportesHidricos";
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, true);

            query = "INSERT INTO " + table + "(Nombre, Periodo, Valor, Escenario) " +
                    "VALUES(@name, @period, @value, @scenario)";
            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@name", OleDbType.VarChar);
                command.Parameters.Add("@period", OleDbType.Numeric);
                command.Parameters.Add("@value", OleDbType.Numeric);
                command.Parameters.Add("@scenario", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                foreach (River river in rivers)
                {
                    if (river != null)
                    {
                        if (river.PeriodicInflows != null)
                        {
                            int period = 0;
                            foreach (double inflow in river.PeriodicInflows)
                            {
                                command.Parameters["@name"].Value = river.Name;
                                command.Parameters["@period"].Value = period + filesReadingParameters.InitialPeriodDB;
                                command.Parameters["@value"].Value = inflow;
                                command.Parameters["@scenario"].Value = scenario;
                                command.ExecuteNonQuery();
                                period++;
                            }
                        }
                    }
                }
                DataBaseManager.DbConnection.Close();
            }
        }

        public void WritePeriodicPlantsMaintenances(string table, List<ConventionalPlant> plants, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, true);

            query = "INSERT INTO " + table + "(nombre, Minimo, CostoVariable, Obligatorio, Escenario, Periodo, Maximo) " +
                    "VALUES(@name, @min, @variableCost, @mandatory, @scenario, @period, @value)";

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@name", OleDbType.VarChar);
                command.Parameters.Add("@min", OleDbType.Numeric);
                command.Parameters.Add("@variableCost", OleDbType.Numeric);
                command.Parameters.Add("@mandatory", OleDbType.Numeric);
                command.Parameters.Add("@scenario", OleDbType.Numeric);
                command.Parameters.Add("@period", OleDbType.Numeric);
                command.Parameters.Add("@value", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                foreach (ConventionalPlant plant in plants)
                {
                    if (plant.PeriodicMaintenances != null)
                    {
                        int period = 0;
                        foreach (double maintenance in plant.PeriodicMaintenances)
                        {
                            command.Parameters["@name"].Value = plant.Name;
                            command.Parameters["@min"].Value = plant.Min;
                            command.Parameters["@variableCost"].Value = plant.VariableCost;
                            command.Parameters["@mandatory"].Value = plant.IsMandatory;
                            command.Parameters["@scenario"].Value = scenario;
                            command.Parameters["@period"].Value = period + filesReadingParameters.InitialPeriodDB;
                            command.Parameters["@value"].Value = maintenance;
                            command.ExecuteNonQuery();
                            period++;
                        }
                    }
                }
                DataBaseManager.DbConnection.Close();
            }
        }

        public void WritePeriodicReservoirsLevels(List<Reservoir> reservoirs, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "embalsePeriodo";
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, true);

            query = "INSERT INTO " + table + "(Nombre, periodo, VolumenMinimo, VolumenMaximo, escenario) " +
                    "VALUES(@name, @period, @min, @max, @scenario)";

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@name", OleDbType.VarChar);
                command.Parameters.Add("@period", OleDbType.Numeric);
                command.Parameters.Add("@min", OleDbType.Numeric);
                command.Parameters.Add("@max", OleDbType.Numeric);
                command.Parameters.Add("@scenario", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                foreach (Reservoir reservoir in reservoirs)
                {
                    int period = 0;
                    if (reservoir.PeriodicLevels != null)
                    {
                        foreach (PeriodicLevels periodicLevels in reservoir.PeriodicLevels)
                        {
                            if (periodicLevels != null)
                            {
                                command.Parameters["@name"].Value = reservoir.Name;
                                command.Parameters["@period"].Value = period + filesReadingParameters.InitialPeriodDB;
                                command.Parameters["@min"].Value = periodicLevels.Min;
                                command.Parameters["@max"].Value = periodicLevels.Max;
                                command.Parameters["@scenario"].Value = scenario;
                                command.ExecuteNonQuery();
                            }
                            period++;
                        }
                    }
                }
                DataBaseManager.DbConnection.Close();
            }
        }

        public void UpdateFuelsCosts(List<Fuel> fuels)
        {
            string table = "combustibleBasica";

            query = "UPDATE " + table + " SET CostoCombustible = @cost " +
                    "WHERE CentroAbastecimiento = @supplyCenter";

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@cost", OleDbType.Numeric);
                command.Parameters.Add("@supplyCenter", OleDbType.VarChar);

                DataBaseManager.DbConnection.Open();

                foreach (Fuel fuel in fuels)
                {
                    command.Parameters["@cost"].Value = fuel.Cost;
                    command.Parameters["@supplyCenter"].Value = fuel.Name;
                    command.ExecuteNonQuery();   
                }
                DataBaseManager.DbConnection.Close();
            }
        }

        public void WritePeriodicFuelsCostsAndCapacity(List<Fuel> fuels, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "combustiblePeriodo";
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, true);

            query = "INSERT INTO " + table + "(CentroAbastecimiento, Periodo, CapacidadHora, MinimoHora, CostoCombustible, CostoTransporte, Escenario) " +
                    "VALUES(@supplyCenter, @period, @capacity, @min, @cost, @transportCost, @scenario)";

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@supplyCenter", OleDbType.VarChar);
                command.Parameters.Add("@period", OleDbType.Numeric);
                command.Parameters.Add("@capacity", OleDbType.Numeric);
                command.Parameters.Add("@min", OleDbType.Numeric);
                command.Parameters.Add("@cost", OleDbType.Numeric);
                command.Parameters.Add("@transportCost", OleDbType.Numeric);
                command.Parameters.Add("@scenario", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                foreach (Fuel fuel in fuels)
                {
                    if (fuel.PeriodicCostsAndCapacity != null)
                    {
                        int period = 0;
                        foreach (PeriodicCostsAndCapacity periodicData in fuel.PeriodicCostsAndCapacity)
                        {
                            if (periodicData != null)
                            {
                                command.Parameters["@supplyCenter"].Value = fuel.Name;
                                command.Parameters["@period"].Value = period + filesReadingParameters.InitialPeriodDB; 
                                command.Parameters["@capacity"].Value = periodicData.Capacity;
                                command.Parameters["@min"].Value = periodicData.Min;
                                command.Parameters["@cost"].Value = periodicData.Cost;
                                command.Parameters["@transportCost"].Value = periodicData.TransportCost;
                                command.Parameters["@scenario"].Value = scenario;
                                command.ExecuteNonQuery();
                            }
                            period++;
                        }
                    }
                }
                DataBaseManager.DbConnection.Close();
            }
        }


        public void UpdateFuelContracts(List<FuelContract> fuelContracts)
        {
            string table = "ContratoCombustibleBasica";

            query = "UPDATE " + table + " SET CapacidadHora = @capacity, CostoContrato = @cost, EtapaFinal = @finalPeriod " +
                    "WHERE Nombre = @name";

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@capacity", OleDbType.Numeric);
                command.Parameters.Add("@cost", OleDbType.Numeric);
                command.Parameters.Add("@endPeriod", OleDbType.Numeric);
                command.Parameters.Add("@name", OleDbType.VarChar);

                DataBaseManager.DbConnection.Open();

                foreach (FuelContract fuelContract in fuelContracts)
                {
                    command.Parameters["@capacity"].Value = fuelContract.Capacity;
                    command.Parameters["@cost"].Value = fuelContract.Cost;
                    command.Parameters["@endPeriod"].Value = fuelContract.FinalPeriod;
                    command.Parameters["@name"].Value = fuelContract.Name;
                    command.ExecuteNonQuery();
                }
                DataBaseManager.DbConnection.Close();
            }
        }

        public void WritePeriodicFuelContractsCostsAndCapacity(List<FuelContract> fuelContracts, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "ContratoCombustiblePeriodo";
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, true);

            query = "INSERT INTO " + table + "(Nombre, Periodo, CapacidadHora, MinimoHora, CostoContrato, Escenario) " +
                    "VALUES(@name, @period, @capacity, @min, @cost, @scenario)";

            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@name", OleDbType.VarChar);
                command.Parameters.Add("@period", OleDbType.Numeric);
                command.Parameters.Add("@capacity", OleDbType.Numeric);
                command.Parameters.Add("@min", OleDbType.Numeric);
                command.Parameters.Add("@cost", OleDbType.Numeric);
                command.Parameters.Add("@scenario", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                foreach (FuelContract fuelContract in fuelContracts)
                {
                    if (fuelContract.PeriodicCostsAndCapacity != null)
                    {
                        int period = 0;
                        foreach (PeriodicCostsAndCapacity periodicData in fuelContract.PeriodicCostsAndCapacity)
                        {
                            if (periodicData != null)
                            {
                                command.Parameters["@name"].Value = fuelContract.Name;
                                command.Parameters["@period"].Value = period + filesReadingParameters.InitialPeriodDB;
                                command.Parameters["@capacity"].Value = periodicData.Capacity;
                                command.Parameters["@min"].Value = periodicData.Min;
                                command.Parameters["@cost"].Value = periodicData.Cost;
                                command.Parameters["@scenario"].Value = scenario;
                                command.ExecuteNonQuery();
                            }
                            period++;
                        }
                    }
                }
                DataBaseManager.DbConnection.Close();
            }
        }

        public void UpdateReservoirsLevels(List<Reservoir> reservoirs, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "embalseBasica";
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, false);

            if (ScenarioExists(table, scenario))
            {
                query = "UPDATE " + table + " SET VolMinimo = @minLevel, VolMaximo = @maxLevel, VolumenInicial = @initialLevel, VolumenFinal = @finalLevel " +
                        " WHERE Nombre = @name AND Escenario = " + scenario;

                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    command.Parameters.Add("@minLevel", OleDbType.Numeric);
                    command.Parameters.Add("@maxLevel", OleDbType.Numeric);
                    command.Parameters.Add("@initialLevel", OleDbType.Numeric);
                    command.Parameters.Add("@finalLevel", OleDbType.Numeric);
                    command.Parameters.Add("@name", OleDbType.VarChar);

                    DataBaseManager.DbConnection.Open();

                    foreach (Reservoir reservoir in reservoirs)
                    {
                        command.Parameters["@minLevel"].Value = reservoir.MinLevel;
                        command.Parameters["@maxLevel"].Value = reservoir.MaxLevel;
                        command.Parameters["@initialLevel"].Value = reservoir.InitialLevel;
                        command.Parameters["@finalLevel"].Value = reservoir.FinalLevel;
                        command.Parameters["@name"].Value = reservoir.Name;
                        command.ExecuteNonQuery();
                    }
                    DataBaseManager.DbConnection.Close();
                }
            }
            else
            {
                query = "INSERT INTO " + table + "(Filtracion, FactorRecuperacion, FactorPenalizacionVertimiento, empresa, EtapaEntrada, Nombre, VolMinimo, VolMaximo, VolumenInicial, VolumenFinal, Escenario)" +
                        " SELECT e.Filtracion, e.FactorRecuperacion, e.FactorPenalizacionVertimiento, e.empresa, e.EtapaEntrada, @name, @minLevel, @maxLevel, @initialLevel, @finalLevel, @scenario" +
                        " FROM " + table + " e" +
                        " WHERE Nombre = @name AND Escenario = 1";

                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    command.Parameters.Add("@name", OleDbType.VarChar);
                    command.Parameters.Add("@minLevel", OleDbType.Numeric);
                    command.Parameters.Add("@maxLevel", OleDbType.Numeric);
                    command.Parameters.Add("@initialLevel", OleDbType.Numeric);
                    command.Parameters.Add("@finalLevel", OleDbType.Numeric);
                    command.Parameters.Add("@scenario", OleDbType.Numeric);

                    DataBaseManager.DbConnection.Open();

                    foreach (Reservoir reservoir in reservoirs)
                    {
                        command.Parameters["@name"].Value = reservoir.Name;
                        command.Parameters["@minLevel"].Value = reservoir.MinLevel;
                        command.Parameters["@maxLevel"].Value = reservoir.MaxLevel;
                        command.Parameters["@initialLevel"].Value = reservoir.InitialLevel;
                        command.Parameters["@finalLevel"].Value = reservoir.FinalLevel;
                        command.Parameters["@scenario"].Value = scenario;
                        command.ExecuteNonQuery();
                    }
                    DataBaseManager.DbConnection.Close();
                }
            }

        }

        public bool ScenarioExists(string table, int scenario)
        {
            bool scenarioExists;

            string queryTmp = "SELECT DISTINCT(Escenario) FROM " + table +
                    " WHERE Escenario = " + scenario;
            reader = DataBaseManager.ReadData(queryTmp);
            if (reader.Read())
                scenarioExists = true;
            else
                scenarioExists = false;

            DataBaseManager.DbConnection.Close();

            return scenarioExists;
        }

        public void UpdateHydroPlantsProductionFactorAndMax(List<ConventionalPlant> hydroPlants, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "recursoHidroBasica";

            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, false);

            if (ScenarioExists(table, scenario))
            {
                query = "UPDATE " + table + " SET Maximo = @max, FactorConversionPromedio = @productionFactor " +
                        "WHERE Nombre = @name AND Escenario = " + scenario;

                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    command.Parameters.Add("@max", OleDbType.Numeric);
                    command.Parameters.Add("@productionFactor", OleDbType.Numeric);
                    command.Parameters.Add("@name", OleDbType.VarChar);

                    DataBaseManager.DbConnection.Open();

                    foreach (ConventionalPlant hydroPlant in hydroPlants)
                    {
                        command.Parameters["@max"].Value = hydroPlant.Max;
                        command.Parameters["@productionFactor"].Value = hydroPlant.ProductionFactor;
                        command.Parameters["@name"].Value = hydroPlant.Name;
                        command.ExecuteNonQuery();
                    }
                    DataBaseManager.DbConnection.Close();
                }
            }
            else
            {
                query = "INSERT INTO " + table + "(FactorDisponibilidad, Minimo, CostoVariable, PorcentajeAGC, FactorConversionVariable, empresa, Obligatorio, EtapaEntrada, Subarea, nombre, FactorConversionPromedio, Maximo, Escenario)" +
                        " SELECT r.FactorDisponibilidad, r.Minimo, r.CostoVariable, r.PorcentajeAGC, r.FactorConversionVariable, r.empresa, r.Obligatorio, r.EtapaEntrada, r.Subarea, @name, @productionFactor, @max, @scenario" +
                        " FROM " + table + " r" +
                        " WHERE Nombre = @name AND Escenario = 1";

                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    command.Parameters.Add("@name", OleDbType.VarChar);
                    command.Parameters.Add("@productionFactor", OleDbType.Numeric);
                    command.Parameters.Add("@max", OleDbType.Numeric);
                    command.Parameters.Add("@scenario", OleDbType.Numeric);

                    DataBaseManager.DbConnection.Open();

                    foreach (ConventionalPlant hydroPlant in hydroPlants)
                    {
                        command.Parameters["@name"].Value = hydroPlant.Name;
                        command.Parameters["@productionFactor"].Value = hydroPlant.ProductionFactor;
                        command.Parameters["@max"].Value = hydroPlant.Max;
                        command.Parameters["@scenario"].Value = scenario;
                        command.ExecuteNonQuery();
                    }
                    DataBaseManager.DbConnection.Close();
                }
            }
        }

        public void WriteBasicPeriod(Period[] basicPeriod, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "PeriodoBasica";
            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, true);

            query = "INSERT INTO " + table + "(Fecha, nombre, demanda, duracionHoras, ReservaAGC, CostoRacionamiento, CAR, demandaInternacional, tasaDescuento, Escenario) " +
                    "VALUES(@date, @name, @load, @durationHours, 0, @rationingCost, @CAR, 0, 0, @scenario)";
            using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
            {
                command.Parameters.Add("@date", OleDbType.VarChar);
                command.Parameters.Add("@name", OleDbType.Numeric);
                command.Parameters.Add("@load", OleDbType.Numeric);
                command.Parameters.Add("@durationHours", OleDbType.Numeric);
                command.Parameters.Add("@rationingCost", OleDbType.Numeric);
                command.Parameters.Add("@CAR", OleDbType.Numeric);
                command.Parameters.Add("@scenario", OleDbType.Numeric);

                DataBaseManager.DbConnection.Open();

                foreach (Period period in basicPeriod)
                {
                    command.Parameters["@date"].Value = period.DateFR.ToString("dd/MM/yyyy");
                    command.Parameters["@name"].Value = period.Name + filesReadingParameters.InitialPeriodDB - 1;
                    command.Parameters["@load"].Value = period.Load;
                    command.Parameters["@durationHours"].Value = period.HourlyDuration;
                    command.Parameters["@rationingCost"].Value = period.RationingCost;
                    command.Parameters["@CAR"].Value = period.CAR;
                    command.Parameters["@scenario"].Value = scenario;
                    command.ExecuteNonQuery();
                }

                DataBaseManager.DbConnection.Close();
            }
        }

        public void UpdateThermalPlantsParameters(List<ConventionalPlant> thermalPlants, bool createScenario, bool automaticScenarioCreation, int scenarioToCreate)
        {
            string table = "recursoTermicoBasica";

            int scenario = GetScenario(createScenario, automaticScenarioCreation, scenarioToCreate, table, false);

            if (ScenarioExists(table, scenario))
            {
                query = "UPDATE " + table + " SET FactorDisponibilidad = @availabilityFactor, FactorConsumoPromedio = @productionFactor, Minimo = @min, Maximo = @max, CostoVariable = @variableCost " +
                        "WHERE Nombre = @name AND Escenario = " + scenario;

                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    command.Parameters.Add("@availabilityFactor", OleDbType.Numeric);
                    command.Parameters.Add("@productionFactor", OleDbType.Numeric);
                    command.Parameters.Add("@min", OleDbType.Numeric);
                    command.Parameters.Add("@max", OleDbType.Numeric);
                    command.Parameters.Add("@variableCost", OleDbType.Numeric);
                    command.Parameters.Add("@name", OleDbType.VarChar);

                    DataBaseManager.DbConnection.Open();

                    foreach (ConventionalPlant thermalPlant in thermalPlants)
                    {
                        command.Parameters["@availabilityFactor"].Value = thermalPlant.AvailabilityFactor;
                        command.Parameters["@productionFactor"].Value = thermalPlant.ProductionFactor;
                        command.Parameters["@min"].Value = thermalPlant.Min;
                        command.Parameters["@max"].Value = thermalPlant.Max;
                        command.Parameters["@variableCost"].Value = thermalPlant.VariableCost;
                        command.Parameters["@name"].Value = thermalPlant.Name;
                        command.ExecuteNonQuery();
                    }
                    DataBaseManager.DbConnection.Close();
                }
            }
            else
            {
                query = "INSERT INTO " + table + "(Tipo, FactorConsumoVariable, Obligatorio, empresa, EtapaEntrada, Subarea, Combustible, nombre, FactorDisponibilidad, FactorConsumoPromedio, Minimo, Maximo, CostoVariable, Escenario)" +
                        " SELECT r.Tipo, r.FactorConsumoVariable, r.Obligatorio, r.empresa, r.EtapaEntrada, r.Subarea, r.Combustible, @name, @availabilityFactor, @productionFactor, @min, @max, @variableCost, @scenario" +
                        " FROM " + table + " r" +
                        " WHERE Nombre = @name AND Escenario = 1";

                using (OleDbCommand command = new OleDbCommand(query, DataBaseManager.DbConnection))
                {
                    command.Parameters.Add("@name", OleDbType.VarChar);
                    command.Parameters.Add("@availabilityFactor", OleDbType.Numeric);
                    command.Parameters.Add("@productionFactor", OleDbType.Numeric);
                    command.Parameters.Add("@min", OleDbType.Numeric);
                    command.Parameters.Add("@max", OleDbType.Numeric);
                    command.Parameters.Add("@variableCost", OleDbType.Numeric);
                    command.Parameters.Add("@scenario", OleDbType.Numeric);

                    DataBaseManager.DbConnection.Open();

                    foreach (ConventionalPlant thermalPlant in thermalPlants)
                    {
                        command.Parameters["@name"].Value = thermalPlant.Name;
                        command.Parameters["@availabilityFactor"].Value = thermalPlant.AvailabilityFactor;
                        command.Parameters["@productionFactor"].Value = thermalPlant.ProductionFactor;
                        command.Parameters["@min"].Value = thermalPlant.Min;
                        command.Parameters["@max"].Value = thermalPlant.Max;
                        command.Parameters["@variableCost"].Value = thermalPlant.VariableCost;
                        command.Parameters["@scenario"].Value = scenario;
                        command.ExecuteNonQuery();
                    }
                    DataBaseManager.DbConnection.Close();
                }
            }
        }
    }
}

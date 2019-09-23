using DHOG_WPF.Models;
using DHOG_WPF.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace DHOG_WPF.DataAccess
{
    class AccessDBReader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccessDBReader));
        private OleDbDataReader reader;
        private string query;

        public Dictionary<int, string> ReadRiversMapping()
        {
            Dictionary<int, string> mappingTable = new Dictionary<int, string>();
            query = "SELECT Numero, Rio " +
                    "FROM MapeoRios";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                mappingTable.Add(Convert.ToInt32(reader.GetValue(0)), reader.GetString(1));
            
            DataBaseManager.DbConnection.Close();

            return mappingTable;
        }

        public Dictionary<string, ConventionalPlant> ReadConventionalPlantsMapping(string table, List<ConventionalPlant> ConventionalPlants)
        {
            Dictionary<string, ConventionalPlant> mappingTable = new Dictionary<string, ConventionalPlant>();
            query = "SELECT * FROM " + table;
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                ConventionalPlant ConventionalPlant;
                try
                {
                    ConventionalPlant = ConventionalPlants.First(p => p.Name == reader.GetString(1));
                    mappingTable.Add(reader.GetString(0), ConventionalPlant);
                }
                catch
                {
                    log.Warn(MessageUtil.FormatMessage("WARNING.ConventionalPlantMappingEntryNotFound", reader.GetString(1)));
                }
            }

            List<ConventionalPlant> mappedConventionalPlants = mappingTable.Values.ToList();
            foreach (ConventionalPlant ConventionalPlant in ConventionalPlants)
            try
            {
                mappedConventionalPlants.First(p => p.Name == ConventionalPlant.Name);
            }
            catch
            {
                log.Warn(MessageUtil.FormatMessage("WARNING.PlantNotFoundInMappingTable", ConventionalPlant.Name));
            }

            DataBaseManager.DbConnection.Close();
            
            return mappingTable;
        }

        public bool ScenarioExists(string table, int scenario)
        {
            bool scenarioExists;

            string queryTmp = string.Format("SELECT Escenario FROM {0} " +
                                            "WHERE Escenario = {1}", table, scenario);
            reader = DataBaseManager.ReadData(queryTmp);
            if (reader.Read())
                scenarioExists = true;
            else
                scenarioExists = false;

            DataBaseManager.DbConnection.Close();

            return scenarioExists;
        }

        public List<ConventionalPlant> ReadConventionalPlants(string table, int scenario)
        {
            List<ConventionalPlant> ConventionalPlants = new List<ConventionalPlant>();
            
            if(table.Equals("recursoHidroBasica"))
                query = "SELECT nombre, Minimo, Maximo, costoVariable, obligatorio, FactorConversionPromedio, FactorDisponibilidad FROM " + table;
            else if(table.Equals("recursoTermicoBasica"))
                query = "SELECT nombre, Minimo, Maximo, costoVariable, obligatorio, FactorConsumoPromedio, FactorDisponibilidad FROM " + table;

            if (ScenarioExists(table, scenario))
                query += " WHERE Escenario = " + scenario;
            else
                query += " WHERE Escenario = 1";

            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                ConventionalPlants.Add(new ConventionalPlant(reader.GetString(0), Convert.ToDouble(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4)), Convert.ToDouble(reader.GetValue(5)), Convert.ToDouble(reader.GetValue(6))));

            DataBaseManager.DbConnection.Close();

            return ConventionalPlants;
        }

        public List<Reservoir> ReadReservoirs(int scenario)
        {
            string table = "EmbalseBasica";
            List<Reservoir> reservoirs = new List<Reservoir>();

            query = "SELECT nombre, VolMinimo, VolMaximo, VolumenInicial FROM " + table;

            if (ScenarioExists(table, scenario))
                query += " WHERE Escenario = " + scenario;
            else
                query += " WHERE Escenario = 1";

            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                reservoirs.Add(new Reservoir(reader.GetString(0), Convert.ToDouble(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToDouble(reader.GetValue(3))));

            DataBaseManager.DbConnection.Close();
            
            return reservoirs;
        }

        public Dictionary<string, Reservoir> ReadReservoirsMapping(List<Reservoir> reservoirs)
        {
            Dictionary<string, Reservoir> mappingTable = new Dictionary<string, Reservoir>();
            query = "SELECT * FROM MapeoEmbalses";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
            {
                Reservoir reservoir;
                try
                {
                    reservoir = reservoirs.First(r => r.Name == reader.GetString(1));
                    mappingTable.Add(reader.GetString(0), reservoir);
                }
                catch
                {
                    log.Warn(MessageUtil.FormatMessage("WARNING.ReservoirMappingEntryNotFound", reader.GetString(1)));
                }
            }

            List<Reservoir> mappedReservoirs = mappingTable.Values.ToList();
            foreach (Reservoir reservoir in reservoirs)
            {
                try
                {
                    mappedReservoirs.First(d => d.Name == reservoir.Name);
                }
                catch
                {
                    log.Warn(MessageUtil.FormatMessage("WARNING.ReservoirNotFoundInMappingTable", reservoir.Name));
                }
            }
                
            DataBaseManager.DbConnection.Close();

            return mappingTable;
        }

        public List<Fuel> ReadFuels()
        {
            List<Fuel> fuels = new List<Fuel>();

            query = "SELECT DISTINCT(CentroAbastecimiento), CapacidadHora, CostoCombustible FROM combustibleBasica";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                fuels.Add(new Fuel(reader.GetString(0), Convert.ToDouble(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2))));

            DataBaseManager.DbConnection.Close();

            return fuels;
        }

        public List<FuelContract> ReadFuelContracts()
        {
            List<FuelContract> fuelContracts = new List<FuelContract>();

            query = "SELECT DISTINCT(Nombre), CapacidadHora, CostoContrato, EtapaInicial, EtapaFinal FROM ContratoCombustibleBasica";
            reader = DataBaseManager.ReadData(query);
            while (reader.Read())
                fuelContracts.Add(new FuelContract(reader.GetString(0), Convert.ToDouble(reader.GetValue(1)), Convert.ToDouble(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4))));

            DataBaseManager.DbConnection.Close();

            return fuelContracts;
        }
    }
}

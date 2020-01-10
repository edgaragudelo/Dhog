using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.DataTypes
{
    public static class Types
    {
        public enum Model
        {
            Short,
            Middle,
            Long
        }

        public enum FileType
        {
            Basic,
            Periodical
        }

        public enum LevelType
        {
            Min,
            Max
        }

        public enum InfoType
        {
            Grid,
            Chart
        }

        public enum SINGenerationType
        {
            Daily,
            Hourly,
            Period,
            Company
        }

        public enum ChartOptionsType
        {
            Scenario,
            Company,
            Reservoir,
            Bar,
            CandleStick
        }

        public enum EntityType
        {
            Company,
            HydroPlant,
            ThermalPlant,
            PeriodicInflow,
            Zone,
            Area,
            Period,
            PeriodicArea,
            Block,
            PeriodicBlock,
            Fuel,
            PeriodicFuel,
            FuelContract,
            RecursoFuelContract,
            PeriodicFuelContract,
            PeriodicLoadBlock,
            HydroElement,
            PeriodicHydroElement,
            Reservoir,
            PeriodicReservoir,
            PeriodicCompany,
            Scenario,
            PeriodicHydroPlant,
            NonConventionalPlant,
            NonConventionalPlantBlock,
            PeriodicNonConventionalPlant,
            PeriodicThermalPlant,
            HydroSystem,
            PeriodicHydroSystem,
            VariableHydroPlant,
            VariableThermalPlant,
            PFEquation,
            ExcludingPlants,
            HydroTopology,
            PeriodicZone,
            PeriodicBarra,
            AreaBarra,
            LineaPeriodo,
            LineaBarra,
            CorteLinea,
            CortePeriodo,
            UnidadPeriodo,
            RecursoPeriodo,
            RecursoPrecio,
            RecursoBasica,
            RecursoFactible,
            RecursoUnidad,
            UnidadBarra,
            RecursoRampa,
            ZonaUnidad
        }
    }
}

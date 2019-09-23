using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class ConventionalPlant : Plant
    {
        /* Constructor needed by the FilesReader module */
        public ConventionalPlant(string name, double min, double max, double variableCost, int mandatory, double productionFactor, double availabilityFactor)
        {
            Name = name;
            Min = min;
            Max = max;
            VariableCost = variableCost;
            IsMandatory = mandatory;
            ProductionFactor = productionFactor;
            AvailabilityFactor = availabilityFactor;
        }

        public ConventionalPlant() { }

        public double AvailabilityFactor { get; set; }
        public double Min { get; set; }
        public double VariableCost { get; set; }
        public double HasVariableProductionFactor { get; set; }
        public int IsMandatory { get; set; }

        /* Property needed by the FilesReader module */
        public double[] PeriodicMaintenances { get; set; }
    }
}

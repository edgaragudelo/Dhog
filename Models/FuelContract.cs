using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class FuelContract : BasicEntity
    {
        /* Constructor needed by the FilesReader module */
        public FuelContract(string name, double capacity, double cost, int initialPeriod, int endPeriod)
        {
            Name = name;
            Cost = cost;
            Capacity = capacity;
            InitialPeriod = initialPeriod;
            FinalPeriod= endPeriod;
        }

        public FuelContract() { }

        public string Type { get; set; }
        public double Capacity { get; set; }
        public double Min { get; set; }
        public double Cost { get; set; }
        public int InitialPeriod { get; set; }
        public int FinalPeriod{ get; set; }

        /* Property needed by the FilesReader module */
        public PeriodicCostsAndCapacity[] PeriodicCostsAndCapacity { get; set; }
    }
}

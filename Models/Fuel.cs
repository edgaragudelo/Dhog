using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Models
{
    public class Fuel : BasicEntity
    {
        /* Constructor needed by the FilesReader module */
        public Fuel(string name, double capacity, double cost)
        {
            Name = name;
            Capacity = capacity;
            Cost = cost;
        }

        public Fuel() { }

        public string Type { get; set; }
        public double Capacity { get; set; }
        public double Min { get; set; }
        public double Cost { get; set; }
        public double TransportCost { get; set; }

        /* Property needed by the FilesReader module */
        public PeriodicCostsAndCapacity[] PeriodicCostsAndCapacity { get; set; }
    }
}

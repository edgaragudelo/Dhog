/* Class needed by the FilesReader module */

namespace DHOG_WPF.Models
{
    public class PeriodicCostsAndCapacity
    {
        public PeriodicCostsAndCapacity(double cost, double capacity)
        {
            Capacity = capacity;
            Min = 0;
            Cost = cost;
            TransportCost = 0;
        }

        public double Capacity { get; set; }
        public double Min { get; }
        public double Cost { get; set; }
        public double TransportCost { get;  }
    }
}

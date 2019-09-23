
namespace DHOG_WPF.Models
{
    public class PeriodicFuel:PeriodicEntity
    {
        public PeriodicFuel(string name, int period, double capacity, double min, double cost, double transportCost, int scenario)
        {
            Name = name;
            Period = period;
            Capacity = capacity;
            Min = min;
            Cost = cost;
            TransportCost = transportCost;
            Case = scenario;
        }

        public double Capacity { get; set; }
        public double Min { get; set; }
        public double Cost { get; set; }
        public double TransportCost { get; set; }
    }
}

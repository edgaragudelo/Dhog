
namespace DHOG_WPF.Models
{
    public class PeriodicFuelContract:PeriodicEntity
    {
        public PeriodicFuelContract(string name, int period, double capacity, double min, double cost, int scenario)
        {
            Name = name;
            Period = period;
            Capacity = capacity;
            Min = min;
            Cost = cost;
            Case = scenario;
        }

        public double Capacity { get; set; }
        public double Min { get; set; }
        public double Cost { get; set; }
    }
}

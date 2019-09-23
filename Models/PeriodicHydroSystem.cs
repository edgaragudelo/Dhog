
namespace DHOG_WPF.Models
{
    public class PeriodicHydroSystem : PeriodicEntity
    {
        public PeriodicHydroSystem(string name, int period, double minTurbinedOutflow, double maxTurbinedOutflow)
        {
            Name = name;
            Period = period;
            MinTurbinedOutflow = minTurbinedOutflow;
            MaxTurbinedOutflow = maxTurbinedOutflow;
        }

        public double MinTurbinedOutflow { get; set; }
        public double MaxTurbinedOutflow { get; set; }
    }
}

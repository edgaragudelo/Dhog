
namespace DHOG_WPF.Models
{
    public class PeriodicHydroElement:PeriodicEntity
    {
        public PeriodicHydroElement(string name, int period, double minTurbinedOutflow, double maxTurbinedOutflow, double filtration, double recoveryFactor)
        {
            Name = name;
            Period = period;
            MinTurbinedOutflow = minTurbinedOutflow;
            MaxTurbinedOutflow = maxTurbinedOutflow;
            Filtration = filtration;
            RecoveryFactor = recoveryFactor;
        }

        public double MinTurbinedOutflow { get; set; }
        public double MaxTurbinedOutflow { get; set; }
        public double Filtration { get; set; }
        public double RecoveryFactor { get; set; }
    }
}

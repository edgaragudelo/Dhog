
namespace DHOG_WPF.Models
{
    public class HydroSystem : BasicEntity
    {
        public HydroSystem() { }

        public double MinTurbinedOutflow { get; set; }
        public double MaxTurbinedOutflow { get; set; }
        public double EnergyFactor { get; set; }
        public int StartPeriod { get; set; }
    }
}

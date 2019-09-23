
namespace DHOG_WPF.Models
{
    public class HydroElement : BasicEntity
    {
        public HydroElement() { }

        public double MinTurbinedOutflow { get; set; }
        public double MaxTurbinedOutflow { get; set; }
        public double Filtration { get; set; }
        public double RecoveryFactor { get; set; }
        public int StartPeriod { get; set; }
    }
}

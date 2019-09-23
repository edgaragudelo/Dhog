
namespace DHOG_WPF.Models
{
    public class PeriodicReservoir:PeriodicEntity
    {
        public PeriodicReservoir(string name, int period, double minLevel, double maxLevel, int scenario)
        {
            Name = name;
            Period = period;
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            Case = scenario;
        }

        public double MinLevel { get; set; }
        public double MaxLevel { get; set; }
    }
}

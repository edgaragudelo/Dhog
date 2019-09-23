
namespace DHOG_WPF.Models
{
    public class PeriodicNonConventionalPlant:PeriodicEntity
    {
        public PeriodicNonConventionalPlant(string name, int period, double max, double plantFactor, int scenario)
        {
            Name = name;
            Period = period;
            Max = max;
            PlantFactor = plantFactor;
            Case = scenario;
        }

        public double Max { get; set; }
        public double PlantFactor { get; set; }
    }
}

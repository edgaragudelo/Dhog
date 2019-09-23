
namespace DHOG_WPF.Models
{
    public class Reservoir : HydroElement
    {
        /* Constructor needed by the FilesReader module */
        public Reservoir(string name, double minLevel, double maxLevel, double initialLevel)
        {
            Name = name;
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            InitialLevel = initialLevel;
            FinalLevel = initialLevel;
        }

        public Reservoir() { }

        public double MinLevel { get; set; }
        public double MaxLevel { get; set; }
        public double InitialLevel { get; set; }
        public double FinalLevel { get; set; }
        public double SpillagePenalizationFactor { get; set; }
        public string Company { get; set; }

        /* Property needed by the FilesReader module */
        public PeriodicLevels[] PeriodicLevels { get; set; }
    }
}

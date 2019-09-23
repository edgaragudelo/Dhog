
namespace DHOG_WPF.Models
{
    public class PeriodicArea : PeriodicEntity
    {
        public PeriodicArea(string name, int period, double load, double importationLimit, double exportationLimit)
        {
            Name = name;
            Period = period;
            Load = load;
            ImportationLimit = importationLimit;
            ExportationLimit = exportationLimit;
        }

        public double Load { get; set; }
        public double ImportationLimit { get; set; }
        public double ExportationLimit { get; set; }
    }
}

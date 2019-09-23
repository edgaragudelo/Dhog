
namespace DHOG_WPF.Models
{
    public class PeriodicConventionalPlant:PeriodicEntity
    {
        public PeriodicConventionalPlant(string name, int period, double variableCost, double min, double max, int mandatory, int scenario)
        {
            Name = name;
            Period = period;
            VariableCost = variableCost;
            Min = min;
            Max = max;
            IsMandatory = mandatory;
            Case = scenario;
        }

        public double VariableCost { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public int IsMandatory { get; set; }
    }
}

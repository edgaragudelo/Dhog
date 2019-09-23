
namespace DHOG_WPF.Models
{
    public class PeriodicInflow: PeriodicEntity
    {
        public PeriodicInflow(string name, int period, double value, int scenario)
        {
            Name = name;
            Period = period;
            Value = value;
            Case = scenario;
        }

        public double Value { get; set; }
    }
}

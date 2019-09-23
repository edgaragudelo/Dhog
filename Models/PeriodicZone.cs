
namespace DHOG_WPF.Models
{
    public class PeriodicZone : PeriodicEntity
    {
        public PeriodicZone(string name, int period, string type, double value)
        {
            Name = name;
            Period = period;
            Type = type;
            Value = value;
        }

        public string Type { get; set; }
        public double Value { get; set; }
    }
}

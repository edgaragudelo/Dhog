/* Class needed by the FilesReader module */

namespace DHOG_WPF.Models
{
    public class PeriodicLevels
    {
        public PeriodicLevels(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; set; }
        public double Max { get; set; }
    }
}

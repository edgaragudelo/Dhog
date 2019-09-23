
namespace DHOG_WPF.Models
{
    public class PeriodicBlock : PeriodicEntity
    {
        public PeriodicBlock(int block, int period, double durationFactor, double loadFactor)
        {
            Block = block;
            Period = period;
            DurationFactor = durationFactor;
            LoadFactor = loadFactor;
        }

        public int Block { get; set; }
        public double DurationFactor { get; set; }
        public double LoadFactor { get; set; }
    }
}
